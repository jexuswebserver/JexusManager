// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Authentication
{
    internal sealed class AuthenticationModuleProxy : ModuleServiceProxy
    {
        internal bool GetAnonymousEnabled()
        {
            return (bool)Invoke(nameof(GetAnonymousEnabled));
        }

        internal void SetAnonymousEnabled(bool enabled)
        {
            Invoke(nameof(SetAnonymousEnabled), enabled);
        }

        internal AnonymousItem GetAnonymousSettings()
        {
            return (AnonymousItem)Invoke(nameof(GetAnonymousSettings));
        }

        internal void ApplyAnonymous(AnonymousItem settings)
        {
            Invoke(nameof(ApplyAnonymous), settings);
        }

        internal bool GetBasicEnabled()
        {
            return (bool)Invoke(nameof(GetBasicEnabled));
        }

        internal void SetBasicEnabled(bool enabled)
        {
            Invoke(nameof(SetBasicEnabled), enabled);
        }

        internal BasicItem GetBasicSettings()
        {
            return (BasicItem)Invoke(nameof(GetBasicSettings));
        }

        internal void ApplyBasic(BasicItem settings)
        {
            Invoke(nameof(ApplyBasic), settings);
        }

        internal bool GetClientCertificateEnabled()
        {
            return (bool)Invoke(nameof(GetClientCertificateEnabled));
        }

        internal void SetClientCertificateEnabled(bool enabled)
        {
            Invoke(nameof(SetClientCertificateEnabled), enabled);
        }

        internal bool GetDigestEnabled()
        {
            return (bool)Invoke(nameof(GetDigestEnabled));
        }

        internal void SetDigestEnabled(bool enabled)
        {
            Invoke(nameof(SetDigestEnabled), enabled);
        }

        internal DigestItem GetDigestSettings()
        {
            return (DigestItem)Invoke(nameof(GetDigestSettings));
        }

        internal void ApplyDigest(DigestItem settings)
        {
            Invoke(nameof(ApplyDigest), settings);
        }

        internal bool GetFormsEnabled()
        {
            return (bool)Invoke(nameof(GetFormsEnabled));
        }

        internal void SetFormsEnabled(bool enabled)
        {
            Invoke(nameof(SetFormsEnabled), enabled);
        }

        internal FormsItem GetFormsSettings()
        {
            return (FormsItem)Invoke(nameof(GetFormsSettings));
        }

        internal void ApplyForms(FormsItem settings)
        {
            Invoke(nameof(ApplyForms), settings);
        }

        internal bool GetImpersonationEnabled()
        {
            return (bool)Invoke(nameof(GetImpersonationEnabled));
        }

        internal void SetImpersonationEnabled(bool enabled)
        {
            Invoke(nameof(SetImpersonationEnabled), enabled);
        }

        internal ImpersonationItem GetImpersonationSettings()
        {
            return (ImpersonationItem)Invoke(nameof(GetImpersonationSettings));
        }

        internal void ApplyImpersonation(ImpersonationItem settings)
        {
            Invoke(nameof(ApplyImpersonation), settings);
        }

        internal bool GetWindowsEnabled()
        {
            return (bool)Invoke(nameof(GetWindowsEnabled));
        }

        internal void SetWindowsEnabled(bool enabled)
        {
            Invoke(nameof(SetWindowsEnabled), enabled);
        }

        internal WindowsItem GetWindowsSettings()
        {
            return (WindowsItem)Invoke(nameof(GetWindowsSettings));
        }

        internal void ApplyWindows(WindowsItem settings)
        {
            Invoke(nameof(ApplyWindows), settings);
        }
    }
}
