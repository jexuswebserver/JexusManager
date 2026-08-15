# Jexus Manager client/server redesign

## Status

This is an architectural proposal, not an implementation specification. It is based on comparing the current Jexus Manager code with the PHP Manager for IIS extension in `C:\Users\lextudio\source\repos\phpmanager`.

## Executive summary

Jexus Manager currently emulates much of the IIS Manager API surface and UI, but it does not implement the boundary that gives that API its architecture. Feature code, configuration objects, and commits all live in the desktop process. A remote Jexus connection downloads configuration into a client-side object graph/cache; edits happen there; `CommitChanges` translates and uploads the result. This is a remote file/configuration adapter, not IIS Manager's client/server model.

The redesign should make the desktop application a true client:

- The client owns navigation, pages, dialogs, presentation state, and typed proxies.
- A management host on the managed machine owns configuration access, filesystem access, certificates, process/service control, validation, authorization, and commits.
- Calls cross an explicit module-service boundary with serializable contracts.
- The same boundary is used for local and remote connections. A local connection may use an in-process channel for efficiency, but client code must not bypass the channel.
- Server-side module discovery decides which client features are offered for a scope and server capability set.

The existing `Microsoft.Web.Management` compatibility assembly is the right conceptual home for this runtime, but today its most important types are placeholders. They should become the compatibility facade over a modern, asynchronous, versioned management protocol.

## What the PHP Manager extension demonstrates

PHP Manager is divided into `Client` and `Server` assemblies, with a small wire-friendly data model shared between them.

The request path is:

```text
PHPPage / dialogs
    -> PHPModule.Proxy
    -> Connection.CreateProxy(...)
    -> PHPModuleProxy.Invoke("method", serializable arguments)
    -> IIS Manager management channel
    -> PHPProvider.ServiceType
    -> PHPService method marked [ModuleServiceMethod]
    -> ManagementUnit scoped to server or site
    -> Microsoft.Web.Administration + filesystem
    -> ManagementUnit.Update()
    -> serializable result / managed error
```

Specific observations:

- `Client/PHPModule.cs` obtains the current `Connection` from the host and creates a `PHPModuleProxy`. The page never constructs the server service or a `ServerManager`.
- `Client/PHPModuleProxy.cs` is deliberately thin. It names an operation and converts shared objects to and from `ArrayList`/primitive data.
- `Server/PHPProvider.cs` binds the logical module name to both the server `PHPService` type and the client module type. `SupportsScope` controls where the module is available.
- `Server/PHPService.cs` is the trust boundary. Public RPC methods are explicitly marked with `[ModuleServiceMethod]`, enforce the allowed `ManagementScope`, use the server-side `ManagementUnit`, and translate errors.
- `Server/Config/ManagementUnitWrapper.cs` commits through `ManagementUnit.Update()`. Therefore the authoritative read, validation, mutation, and write occur on the managed server.
- `Client/RemoteObject.cs` shows an important constraint: objects crossing the boundary are data, not live `Microsoft.Web.Administration` objects.
- Setup registers the provider and module in IIS administration configuration. The server, rather than a hardcoded client list, determines module availability.

IIS Manager also has machinery for downloading a client assembly. Jexus Manager should not enable arbitrary server-supplied code execution in the first implementation. Initially, the server should advertise module IDs and contract versions while the client activates only locally installed, trusted client modules. Signed extension acquisition can be designed separately.

## What Jexus Manager does today

The repository already contains many of the names in the IIS model, but not their behavior:

- `Microsoft.Web.Management/Client/Connection.cs`: `CreateProxy` returns `null`; service-container and configuration-management behavior is unimplemented.
- `Microsoft.Web.Management/Client/ModuleServiceProxy.cs`: `Invoke` returns `null`.
- `Microsoft.Web.Management/Server/ManagementUnit.cs`: module discovery, service resolution, update, and context behavior are unimplemented.
- `Microsoft.Web.Management/Host/ConnectionManager.cs`: activation, login, refresh, persistence, and events are unimplemented.
- Feature providers such as `DefaultDocumentModuleProvider` return `null` for `ServiceType`.
- `MainForm` constructs every module provider in a hardcoded list.
- Tree nodes inject `JexusManager.Services.ConfigurationService` directly into client modules.
- Feature classes use that service to obtain mutable `ConfigurationSection`/`ConfigurationElement` instances and call `service.ServerManager.CommitChanges()` in the desktop process.
- `JexusServerManager` creates a local cache under Documents, reconstructs a `Microsoft.Web.Administration` object graph from REST responses, and uploads translated dictionaries during post-commit.

This creates several structural problems:

1. **No trust boundary.** A page can access anything exposed by the client-side `ServerManager`; server-side scope checks are absent.
2. **Stale writes and lost updates.** The client edits a snapshot and can overwrite changes made after it was loaded.
3. **Leaky platform details.** Paths, certificate operations, process state, and configuration schemas are interpreted on the client even when they belong to another machine.
4. **Coarse commits.** A small UI change can cause broad reconstruction and upload of server/site state.
5. **Extensions are not independently deployable.** Provider, UI module, and configuration logic are compiled into the same feature assembly and assembled by the main executable.
6. **The emulation cannot host a normal IIS Manager extension pattern.** The API names exist, but the proxy/service lifecycle behind them does not.

## Target architecture

```text
JexusManager.Desktop
  UI modules, pages, dialogs
          |
  typed ModuleServiceProxy
          |
  Connection + IManagementChannel
          |  in-process (local) or HTTPS (remote)
========== trust / process boundary =====================
JexusManager.ManagementHost
  authentication + session + scope authorization
  module catalog + RPC dispatcher
          |
  scoped ManagementUnit
          |
  ModuleService implementations
          |
  Microsoft.Web.Administration / Jexus adapters / OS services
          |
  authoritative configuration and runtime state
```

### 1. Contracts and protocol

Add a UI-free, administration-free contracts layer. It should contain:

- protocol request/response envelopes;
- connection handshake and capability DTOs;
- scope descriptors (`server`, `site`, `application`, `path`);
- module descriptors and contract versions;
- structured errors;
- concurrency tokens;
- feature-specific DTOs shared by each client/server pair.

Do not serialize `ConfigurationElement`, `ServerManager`, exceptions, delegates, controls, or arbitrary CLR type names. The PHP Manager `RemoteObject` approach proves the principle, but a new implementation should use explicit, versioned DTOs rather than untyped `ArrayList` payloads.

A request should identify at least:

```text
protocol version
connection/session ID
module ID
operation ID
scope
arguments
expected configuration revision (for writes)
correlation ID
```

Responses should carry a result or a stable error code, human-readable message, optional field errors, current revision, and correlation ID.

### 2. Client runtime

Implement the currently stubbed IIS-like path:

- `ConnectionManager` creates, authenticates, activates, refreshes, and disposes connections.
- `Connection` owns connection metadata, the selected scope/configuration path, discovered modules, and a channel.
- `Connection.CreateProxy` instantiates a proxy and binds it to the connection, logical module name, and scope.
- `ModuleServiceProxy.Invoke` delegates to that bound channel. Preserve a synchronous compatibility method if required by existing pages, but build the channel itself around `Task`, cancellation, and timeouts.
- `HttpManagementChannel` handles HTTPS transport, serialization, correlation, errors, and capability negotiation.
- `InProcessManagementChannel` dispatches through the same server dispatcher for local management. It must not hand a client feature a `ManagementUnit` or `ServerManager`.
- Module activation uses descriptors returned by the server and a trusted local client-module catalog instead of the hardcoded provider list in `MainForm`.

The WinForms host continues to expose UI-only services such as `IControlPanel` and `IManagementUIService`. `IConfigurationService` in its current form should not be available to migrated client modules because it exposes live administration objects.

### 3. Server runtime

Create a management host that can run beside Jexus (and, where useful, in-process for a local connection). Its responsibilities are:

- authenticate the caller using a standard HTTPS authentication scheme;
- establish the caller's roles and allowed scopes;
- negotiate protocol and feature versions;
- discover/register `ModuleProvider` implementations;
- create a fresh scoped `ManagementUnit` per request or short transaction;
- dispatch only methods explicitly marked `[ModuleServiceMethod]`;
- validate argument types and payload limits;
- perform authorization before invocation;
- map expected failures to stable error contracts;
- commit atomically and return a new configuration revision;
- record audit events without logging secrets.

Reflection may be used to build an allowlisted dispatch table at startup. It should never accept an arbitrary CLR type or method name from the network and reflect over it directly.

The management host, not the desktop client, references the authoritative `Microsoft.Web.Administration` implementation and platform-specific services. Jexus-specific translation between IIS-shaped configuration and `jws.conf`/site files also belongs here.

### 4. ManagementUnit and scope

`ManagementUnit` is more than a wrapper around `ServerManager`; it is the per-call security and configuration context. A concrete unit should contain:

- authenticated principal;
- connection/session context;
- `ManagementScope` and canonical scope path;
- read-only and mutable server managers/adapters;
- configuration path and location resolution;
- authorization/capability checks;
- transaction/change set;
- starting configuration revision.

`Update()` should validate the change set, compare the expected revision, commit on the server, and return/record the new revision. A conflict must be reported instead of silently replacing newer state.

The server must independently enforce scope. A client connected at site scope cannot invoke a server-only operation simply by changing request arguments.

### 5. Module shape

Each independently deployable feature should ultimately have three logical parts:

```text
JexusManager.Features.DefaultDocument.Contracts
JexusManager.Features.DefaultDocument.Client
JexusManager.Features.DefaultDocument.Server
```

They need not all become separate projects on day one, but project references must enforce the boundary:

- Contracts: DTOs and contract constants only.
- Client: WinForms pages, presentation models, client `Module`, and typed proxy. No `Microsoft.Web.Administration` reference.
- Server: `ModuleProvider`, `ModuleService`, configuration mapping, validation, and OS access. No WinForms reference.

A typical migrated service would expose business-level operations rather than a generic remote object graph:

```text
GetSettings() -> DefaultDocumentSnapshot { Enabled, Entries, Revision }
ApplyChanges(DefaultDocumentChangeSet, expectedRevision)
Revert(expectedRevision)
```

This preserves server-side validation and permits protocol evolution. It also avoids a chatty RPC for every property access.

### 6. Generic configuration versus feature services

Jexus Manager has many configuration-only features, so implementing a unique low-level CRUD service for every page would be repetitive. The server runtime can provide a constrained generic configuration service for common section operations:

- read a named, permitted section as a neutral snapshot;
- apply a validated patch/change set;
- clear/revert a section at the current scope;
- retrieve schema metadata needed for editors.

This service must be allowlisted by feature/module and scope. It must not become “send any configuration path and arbitrary XML.” Features that touch files, certificates, processes, services, or complex cross-section invariants should use explicit module services.

The typed feature proxy remains valuable even when its server implementation delegates to the generic configuration service.

### 7. Security choices

The current custom authorization header and ad hoc certificate acceptance should not become the new protocol's security model. The design should require:

- TLS for remote connections;
- an explicit authentication mechanism (for example OS-integrated authentication where available or short-lived bearer tokens issued by the management host);
- credentials stored through the operating system credential vault, never in connection files or logs;
- role- and scope-based authorization on every operation;
- certificate trust/pinning represented as connection policy rather than UI callbacks embedded in administration objects;
- replay-resistant sessions and bounded request sizes;
- audit records for mutations and privileged operations;
- no arbitrary assembly download or arbitrary reflection dispatch.

Exact authentication mechanisms can vary by platform, but the channel and server authorization model should not.

### 8. Consistency, transactions, and refresh

Every read snapshot should have a revision/ETag. Every mutation supplies the revision it was based on. If it is stale, the server returns a conflict and the client offers refresh/reapply instead of overwriting the server.

One user-level Apply action should be one server transaction even if it updates multiple configuration sections. Server services should validate the whole change set before writing. Where the underlying platform cannot provide a true transaction, the service should use atomic file replacement and retain enough information for recovery.

Connection refresh should invalidate page snapshots and module/capability metadata deliberately. It should not recreate an undocumented client-side configuration universe.

## Recommended migration sequence

### Phase 0: Characterization and protocol decisions

- Add tests describing current Default Document behavior at server, site, application, and path scopes.
- Inventory operations by feature: pure configuration, filesystem, certificate, service/process, and Jexus-specific.
- Define supported authentication, scope identifiers, error codes, versioning rules, and revision semantics.
- Decide whether the first host is a separate service, a component of the existing Jexus management API, or both behind the same dispatcher abstraction.

### Phase 1: Build one real vertical slice

Use Default Document because it is small but exercises inheritance, ordered collections, enable/disable, apply, and revert.

- Implement contracts for snapshot/change set/errors.
- Implement provider and server service.
- Implement in-process channel and dispatcher first.
- Implement `Connection.CreateProxy` and `ModuleServiceProxy.Invoke` sufficiently for this slice.
- Change the Default Document client feature to use only its proxy.
- Verify that the client feature project no longer references `Microsoft.Web.Administration`.
- Run the same contract tests over in-process and HTTPS channels.

This phase is successful only when no Default Document configuration object crosses into the UI process.

### Phase 2: Remote management host

- Add HTTPS transport, authentication, session/capability handshake, structured errors, revision checks, and audit logging.
- Move Jexus configuration parsing/translation and commits behind the server host.
- Stop using `JexusServerManager`'s Documents cache as the authoritative editing model.
- Exercise disconnects, retries, duplicate requests, stale revisions, partial failures, and server restarts.

### Phase 3: Migrate features by risk group

1. Simple scalar configuration: directory browsing, compression, logging.
2. Ordered collections: MIME maps, handlers, modules, IP restrictions, request filtering.
3. Multi-section/cross-resource features: sites, applications, pools, FastCGI, rewrite.
4. Privileged OS features: certificates, HTTP API, service/process control.

During migration, an adapter may keep legacy pages working, but a connection must not mix client-side and server-side writes within one Apply action.

### Phase 4: Dynamic module catalog and extension SDK

- Replace `MainForm`'s hardcoded provider construction with server module discovery plus a trusted client-module catalog.
- Publish a small SDK/template that mirrors the PHP Manager pattern: Contracts, Client Module/Proxy, Server Provider/Service.
- Add compatibility tests using a small sample extension.
- Consider signed client extension distribution only after trust, isolation, updates, and rollback are designed.

### Phase 5: Remove the old path

- Remove client access to `JexusManager.Services.IConfigurationService` and `ServerManager` from all migrated features.
- Remove client-side Jexus configuration reconstruction and broad post-commit uploads.
- Delete or narrow compatibility stubs once real behavior and tests cover their supported surface.

## Lessons learned from the migration

The migration work also clarified a few practical rules that should guide the redesign:

- The client UI should not own configuration writes directly. Once a page or feature starts mutating configuration sections or committing changes in the desktop process, it has already crossed the trust boundary that the redesign is meant to preserve.
- The service boundary must be scope-aware. Site-scoped updates are different from server-scoped updates, and the correct path is to resolve the effective configuration location in the server-side service rather than relying on the client-side object graph.
- A thin DTO or snapshot contract is preferable to passing live administration objects across layers. It keeps the UI process decoupled from server-specific configuration details and makes regressions easier to test.
- Regression tests are part of the architecture, not an afterthought. A small proxy/service pipeline test often catches the exact class of mistakes that would otherwise surface later as hard-to-debug scope or locking issues.
- The safest migration strategy is to move one feature at a time, keep the UI layer thin, and verify end-to-end behavior before expanding the pattern to the next module.

These lessons reinforce the design direction: the desktop app should remain a presentation and orchestration client, while the management host should own validation, authorization, and authoritative writes.

## Compatibility goal

The initial goal should be **source and conceptual compatibility** with the IIS Manager extension pattern, not binary compatibility with every .NET Framework IIS Manager extension.

PHP Manager targets the classic .NET Framework/GAC-era IIS Manager runtime, while Jexus Manager currently targets modern .NET on Windows. Loading the existing binary unchanged would require reproducing assembly identity, runtime behavior, remoting serialization, hosting, and a large undocumented API surface. That effort would distract from establishing the correct security and process boundary.

A good compatibility target is:

- familiar `Module`, `ModuleProvider`, `ModuleService`, `ModuleServiceProxy`, `ManagementUnit`, and `[ModuleServiceMethod]` concepts;
- enough matching API shape that an extension can be ported or multi-targeted with a small contracts/serialization update;
- behaviorally accurate scope, discovery, dispatch, update, and error semantics;
- a documented modern protocol underneath.

Binary compatibility can be reassessed after a real extension successfully runs through the new model.

## Acceptance criteria

The redesign is working when all of these are true:

- A feature page can run without referencing `Microsoft.Web.Administration`.
- Local and remote connections execute the same `ModuleService` through different channel implementations.
- Server-side authorization rejects an out-of-scope call even if the client is modified.
- A stale Apply produces a conflict rather than a lost update.
- No managed-machine filesystem path is opened directly by the desktop client.
- Server module availability controls what the client offers at each scope.
- An extension built in separate Contracts/Client/Server pieces can be installed without changing `MainForm`.
- Contract tests produce equivalent results over the local and HTTPS channels.
- Disconnects and failed commits do not leave the client believing that uncommitted state is authoritative.

## Decisions to make before implementation

1. Should the remote management host be a new Jexus-side service or evolve the existing REST API? My preference is to reuse its hosting/deployment only if requests are routed through the new scoped dispatcher; adding more resource-specific endpoints to `JexusServerManager` would preserve the current architecture.
2. Which authentication mechanisms must work on Windows and Linux in the first release?
3. Is source compatibility with third-party IIS Manager extensions a product goal, or is the IIS pattern only an internal design model?
4. Will client modules ship only with Jexus Manager initially, or must independently installed extensions work in the first milestone?
5. What is the authoritative configuration revision for Jexus: a file hash, server-maintained monotonic version, or repository/version-store identity?

My recommendation is to answer only the first three before Phase 1. Default Document over an in-process dispatcher can validate the architecture without prematurely solving extension distribution.
