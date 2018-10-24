// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Microsoft.Web.Administration
{
    public partial class JexusServerManager
    {
        internal static readonly Version MinimumServerVersion = new Version("1.0.000530.00");

        private static string s_protocol = "https";
        private string _keyCache;
        private string _certificateCache;

        internal string LogFolder { get; set; }

        internal string Credentials { get; set; }

        public override bool SupportsSni => false;
        public override bool SupportsWildcard => false;

        public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }
        public string AcceptedHash { get; internal set; }

        public async Task<string> SaveKeyAsync(string key)
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/server/key/save", key);
                if (response.IsSuccessStatusCode)
                {
                    var result = (string)await response.Content.ReadAsAsync(typeof(string));
                    return result;
                }

                return string.Empty;
            }
        }

        public async Task<string> SaveCertificateAsync(string text)
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/server/cert/save", text);
                if (response.IsSuccessStatusCode)
                {
                    var result = (string)await response.Content.ReadAsAsync(typeof(string));
                    return result;
                }

                return string.Empty;
            }
        }

        public async Task<X509Certificate2> GetCertificateAsync()
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/server/cert", _certificateCache);
                if (response.IsSuccessStatusCode)
                {
                    var result = (X509Certificate2)await response.Content.ReadAsAsync(typeof(X509Certificate2));
                    return result;
                }

                return null;
            }
        }

        public void SetCertificate(string certificate)
        {
            _certificateCache = certificate;
        }

        public void SetKeyFile(string key)
        {
            _keyCache = key;
        }

        internal HttpClient GetClient()
        {
            var requestHandler = new WebRequestHandler();
            requestHandler.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
            var client = new HttpClient(requestHandler) {BaseAddress = new Uri($"{s_protocol}://{HostName}/")};
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-HTTP-Authorization", Credentials);
            client.DefaultRequestHeaders.Add("Time", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            return client;
        }

        public async Task<Version> GetVersionAsync()
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.GetAsync("api/server/version");
                if (response.IsSuccessStatusCode)
                {
                    var succeeded = (string)await response.Content.ReadAsAsync(typeof(string));
                    return new Version(succeeded);
                }

                return null;
            }
        }

        public async Task<string> HelloAsync()
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/server/hello", Environment.MachineName);
                if (response.IsSuccessStatusCode)
                {
                    var succeeded = (string)await response.Content.ReadAsAsync(typeof(string));
                    return succeeded;
                }

                return string.Empty;
            }
        }

        public async Task<string> ByeAsync()
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/server/bye", Environment.MachineName);
                if (response.IsSuccessStatusCode)
                {
                    var succeeded = (string)await response.Content.ReadAsAsync(typeof(string));
                    return succeeded;
                }

                return string.Empty;
            }
        }

        public async Task<bool> LocalhostTestAsync(string path, string random)
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/server/test", path);
                if (response.IsSuccessStatusCode)
                {
                    var value = (string)await response.Content.ReadAsAsync(typeof(string));
                    return value == random;
                }

                return false;
            }
        }

        internal override bool Verify(string path)
        {
            return AsyncHelper.RunSync(() => VerifyAsync(path));
        }

        private async Task<bool> VerifyAsync(string path)
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/site/verify", path);
                if (response.IsSuccessStatusCode)
                {
                    var succeeded = (bool)await response.Content.ReadAsAsync(typeof(bool));
                    return succeeded;
                }

                return false;
            }
        }

        public async Task LoadAsync()
        {
            SortedDictionary<string, List<string>> variables = null;
            if (Credentials == null)
            {
                var lines = File.ReadAllLines("jws.conf");
                variables = new SortedDictionary<string, List<string>>();
                foreach (var line in lines)
                {
                    var index = line.IndexOf('#');
                    var content = index == -1 ? line : line.Substring(0, index);
                    var parts = content.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    var key = parts[0].Trim().ToLowerInvariant();
                    var value = parts[1].Trim();
                    if (variables.ContainsKey(key))
                    {
                        variables[key].Add(value);
                    }
                    else
                    {
                        variables.Add(key, new List<string> { value });
                    }
                }
            }
            else
            {
                using (var client = GetClient())
                {
                    HttpResponseMessage response = await client.GetAsync("api/server/");
                    if (response.IsSuccessStatusCode)
                    {
                        variables = (SortedDictionary<string, List<string>>)await response.Content.ReadAsAsync(typeof(SortedDictionary<string, List<string>>));
                    }
                }
            }

            var newPool = new ApplicationPool(null, ApplicationPools)
            {
                Name = "DefaultAppPool",
                ManagedRuntimeVersion = variables.Load(new List<string> {"v2.0"}, "runtime")[0]
            };
            newPool.ProcessModel.MaxProcesses = long.Parse(variables.Load(new List<string> { "1" }, "httpd.processes")[0]);
            newPool.ProcessModel.UserName = variables.Load(new List<string> { string.Empty }, "httpd.user")[0];
            newPool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
            ApplicationPools.Add(newPool);

            SiteFolder = variables.Load(new List<string> { "siteconf" }, "siteconfigdir")[0];
            LogFolder = variables.Load(new List<string> { "log" }, "sitelogdir")[0];
            _certificateCache = variables.Load(new List<string> { string.Empty }, "certificatefile")[0];
            _keyCache = variables.Load(new List<string> { string.Empty }, "certificatekeyfile")[0];

            IEnumerable<string> sites = null;
            if (Credentials == null)
            {
                sites = Directory.GetFiles(SiteFolder).Where(name => !name.Contains("_"));
            }
            else
            {
                using (var client = GetClient())
                {
                    HttpResponseMessage response = await client.GetAsync("api/site/");
                    if (response.IsSuccessStatusCode)
                    {
                        sites = (List<string>)await response.Content.ReadAsAsync(typeof(List<string>));
                    }
                }
            }

            long count = 0;
            Debug.Assert(sites != null, "sites != null");
            foreach (var file in sites)
            {
                var site = Credentials == null
                               ? new Site(Sites) { Name = Path.GetFileName(file), Id = count }
                               : new Site(Sites) { Name = file, Id = count };
                Sites.Add(site);
                count = await LoadAsync(site, count, file, SiteFolder);
            }

            Extra = variables;
        }

        public async Task SaveAsync()
        {
            var variables = new SortedDictionary<string, List<string>>();
            foreach (var item in Extra)
            {
                variables.Add(item.Key, item.Value);
            }

            var pool = ApplicationPools[0];
            variables.Add("runtime", new List<string> { pool.ManagedRuntimeVersion });
            variables.Add("httpd.processes", new List<string> { pool.ProcessModel.MaxProcesses.ToString() });
            variables.Add("httpd.user", new List<string> { pool.ProcessModel.UserName });
            variables.Add("siteconfigdir", new List<string> { SiteFolder });
            variables.Add("sitelogdir", new List<string> { LogFolder });
            variables.Add("certificatefile", new List<string> { _certificateCache });
            variables.Add("certificatekeyfile", new List<string> { _keyCache });
            if (string.IsNullOrEmpty(HostName))
            {
                var lines = new List<string>();
                foreach (var item in variables)
                {
                    foreach (var line in item.Value)
                    {
                        lines.Add($"{item.Key}={line}");
                    }
                }

                File.WriteAllLines("jws.conf", lines);
            }
            else
            {
                using (var client = GetClient())
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync("api/server/", variables);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                    }
                }
            }
        }

        public async Task<bool> GetStatusAsync()
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.GetAsync("api/server/state");
                if (response.IsSuccessStatusCode)
                {
                    var result = (bool)await response.Content.ReadAsAsync(typeof(bool));
                    return result;
                }

                return false;
            }
        }

        public async Task<bool> StopAsync()
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.GetAsync("api/server/stop");
                if (response.IsSuccessStatusCode)
                {
                    var succeeded = (bool)await response.Content.ReadAsAsync(typeof(bool));
                    return succeeded;
                }

                return false;
            }
        }

        public async Task<bool> StartAsync()
        {
            using (var client = GetClient())
            {
                HttpResponseMessage response = await client.GetAsync("api/server/start");
                if (response.IsSuccessStatusCode)
                {
                    var succeeded = (bool)await response.Content.ReadAsAsync(typeof(bool));
                    return succeeded;
                }

                return false;
            }
        }

        public async Task<long> LoadAsync(Site site, long count, string file, string siteFolder)
        {
            SortedDictionary<string, List<string>> siteVariables = null;
            if (Credentials == null)
            {
                var rows = File.ReadAllLines(file);
                siteVariables = new SortedDictionary<string, List<string>>();
                foreach (var line in rows)
                {
                    var index = line.IndexOf('#');
                    var content = index == -1 ? line : line.Substring(0, index);
                    var parts = content.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    var key = parts[0].Trim().ToLowerInvariant();
                    var value = parts[1].Trim();
                    if (siteVariables.ContainsKey(key))
                    {
                        siteVariables[key].Add(value);
                        continue;
                    }

                    siteVariables.Add(key, new List<string> { value });
                }
            }
            else
            {
                using (var client = GetClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"api/site/{file}");
                    if (response.IsSuccessStatusCode)
                    {
                        siteVariables = (SortedDictionary<string, List<string>>)await response.Content.ReadAsAsync(typeof(SortedDictionary<string, List<string>>));
                    }
                }
            }

            count++;
            var useHttps = bool.Parse(siteVariables.Load(new List<string> { "false" }, "usehttps")[0]);
            var host = siteVariables.Load(new List<string> { "*" }, "hosts", "host")[0];
            var endPoint =
                new IPEndPoint(
                    IPAddress.Parse(
                        siteVariables.Load(new List<string> { IPAddress.Any.ToString() }, "addr", "address")[0]),
                    int.Parse(siteVariables.Load(new List<string> { "80" }, "port")[0]));
            var binding = new Binding(useHttps ? "https" : "http", endPoint + ":" + host, null, null, SslFlags.None, site.Bindings);

            if (useHttps)
            {
                var cert = await GetCertificateAsync();
                binding.CertificateHash = cert.GetCertHash();
            }
            else
            {
                binding.CertificateHash = new byte[0];
            }

            site.Bindings.Add(binding);
            var app = new Application(site.Applications)
            {
                Path = Application.RootPath,
                Name = string.Empty,
                ApplicationPoolName = "DefaultAppPool"
            };
            site.Applications.Add(app);
            await LoadAsync(app, null, null, siteVariables);

            IEnumerable<string> appNames = null;
            if (Credentials == null)
            {
                appNames = Directory.GetFiles(siteFolder);
                appNames = appNames.Where(name => name.StartsWith(file + "_", StringComparison.Ordinal));
            }
            else
            {
                using (var client = GetClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"api/app/{site.Name}");
                    if (response.IsSuccessStatusCode)
                    {
                        appNames = (IEnumerable<string>)await response.Content.ReadAsAsync(typeof(IEnumerable<string>));
                    }
                }
            }

            Debug.Assert(appNames != null, "appNames != null");

            foreach (var appName in appNames)
            {
                var application = new Application(site.Applications)
                {
                    ApplicationPoolName = "DefaultAppPool",
                    Path = appName.ToPath(out var applicationName),
                    Name = applicationName
                };
                site.Applications.Add(application);
                await LoadAsync(application, file, appName, null);
            }

            return count;
        }

        public async Task LoadAsync(Application application, string file, string appName, SortedDictionary<string, List<string>> variables)
        {
            if (variables == null)
            {
                if (Credentials == null)
                {
                    var rows = File.ReadAllLines(file);
                    variables = new SortedDictionary<string, List<string>>();
                    foreach (var line in rows)
                    {
                        var index = line.IndexOf('#');
                        var content = index == -1 ? line : line.Substring(0, index);
                        var parts = content.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 2)
                        {
                            continue;
                        }

                        var key = parts[0].Trim().ToLowerInvariant();
                        var value = parts[1].Trim();
                        if (variables.ContainsKey(key))
                        {
                            variables[key].Add(value);
                            continue;
                        }

                        variables.Add(key, new List<string> { value });
                    }
                }
                else
                {
                    using (var client = GetClient())
                    {
                        HttpResponseMessage response = await client.GetAsync($"api/app/get/{appName}");
                        if (response.IsSuccessStatusCode)
                        {
                            variables = (SortedDictionary<string, List<string>>)await response.Content.ReadAsAsync(typeof(SortedDictionary<string, List<string>>));
                        }
                    }
                }
            }

            variables.Load(new List<string> { "false" }, "usehttps");
            variables.Load(new List<string> { "*" }, "hosts", "host");
            variables.Load(new List<string> { IPAddress.Any.ToString() }, "addr", "address");
            variables.Load(new List<string> { "80" }, "port");
            var root = variables.Load(new List<string> { "/ /var/www/default" }, "root")[0];
            var split = root.IndexOf(' ');
            if (split == -1 || split == 0)
            {
                throw new ServerManagerException("invalid root mapping");
            }

            var virtualDirectory =
                new VirtualDirectory(null, application.VirtualDirectories)
                {
                    Path = "/",
                    PhysicalPath = root.Substring(split + 1)
                };
            application.VirtualDirectories.Add(virtualDirectory);
            var configuration = application.GetWebConfiguration();
            var defaultDocument = configuration.GetSection("system.webServer/defaultDocument");
            defaultDocument["enabled"] = true;
            var collection = defaultDocument.GetCollection("files");
            collection.Clear();
            var names = variables.Load(new List<string> { Constants.DefaultDocumentList }, "indexes", "indexs")[0];
            var pageNames = names.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var name in pageNames)
            {
                var file1 = collection.CreateElement();
                file1.Attributes["value"].Value = name;
                collection.Add(file1);
            }


            ConfigurationSection httpLoggingSection = application.Server.GetApplicationHostConfiguration().GetSection("system.webServer/httpLogging", application.Location);
            var dontLog = Convert.ToBoolean(variables.Load(new List<string> { "false" }, "nolog")[0]);
            httpLoggingSection["dontLog"] = dontLog;

            var ipSecuritySection = application.Server.GetApplicationHostConfiguration().GetSection("system.webServer/security/ipSecurity", application.Location);
            ipSecuritySection["enableReverseDns"] = false;
            ipSecuritySection["allowUnlisted"] = true;

            ConfigurationElementCollection ipSecurityCollection = ipSecuritySection.GetCollection();
            ipSecurityCollection.Clear();
            var deny = variables.Load(new List<string>(), "denyfrom", "ip.deny");
            foreach (var denyEntry in deny)
            {
                var denyItems = denyEntry.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in denyItems)
                {
                    ConfigurationElement addElement = ipSecurityCollection.CreateElement("add");
                    if (item.Contains("/"))
                    {
                        var parts = item.Split('/');
                        addElement["ipAddress"] = parts[0];
                        addElement["subnetMask"] = parts[1];
                    }
                    else
                    {
                        addElement["ipAddress"] = item;
                    }

                    addElement["allowed"] = false;
                    ipSecurityCollection.Add(addElement);
                }
            }

            var allow = variables.Load(new List<string>(), "allowfrom", "ip.allow");
            foreach (var allowEntry in allow)
            {
                var allowItems = allowEntry.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in allowItems)
                {
                    ConfigurationElement addElement = ipSecurityCollection.CreateElement("add");
                    if (item.Contains("/"))
                    {
                        var parts = item.Split('/');
                        addElement["ipAddress"] = parts[0];
                        addElement["subnetMask"] = parts[1];
                    }
                    else
                    {
                        addElement["ipAddress"] = item;
                    }

                    addElement["allowed"] = true;
                    ipSecurityCollection.Add(addElement);
                }
            }

            ConfigurationSection requestFilteringSection = configuration.GetSection("system.webServer/security/requestFiltering");
            ConfigurationElement hiddenSegmentsElement = requestFilteringSection.ChildElements["hiddenSegments"];
            ConfigurationElementCollection hiddenSegmentsCollection = hiddenSegmentsElement.GetCollection();
            hiddenSegmentsCollection.Clear();
            var hidden = variables.Load(new List<string> { string.Empty }, "denydirs")[0];
            var hiddenItems = hidden.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in hiddenItems)
            {
                ConfigurationElement add = hiddenSegmentsCollection.CreateElement("add");
                add["segment"] = item;
                hiddenSegmentsCollection.Add(add);
            }

            ConfigurationSection httpErrorsSection = configuration.GetSection("system.webServer/httpErrors");
            ConfigurationElementCollection httpErrorsCollection = httpErrorsSection.GetCollection();
            httpErrorsCollection.Clear();
            Debug.Assert(variables != null, "variables != null");
            if (variables.ContainsKey("nofile"))
            {
                var error = variables["nofile"][0];
                ConfigurationElement errorElement = httpErrorsCollection.CreateElement("error");
                errorElement["statusCode"] = 404;
                errorElement["subStatusCode"] = 0;
                errorElement["prefixLanguageFilePath"] = @"%SystemDrive%\inetpub\custerr";
                errorElement["responseMode"] = "ExecuteURL";
                errorElement["path"] = error;
                httpErrorsCollection.Add(errorElement);
                variables.Remove("nofile");
            }

            var urlCompressionSection = configuration.GetSection("system.webServer/urlCompression");
            urlCompressionSection["doStaticCompression"] = Convert.ToBoolean(variables.Load(new List<string> { "true" }, "usegzip")[0]);

            ConfigurationSection httpProtocolSection = configuration.GetSection("system.webServer/httpProtocol");
            httpProtocolSection["allowKeepAlive"] = Convert.ToBoolean(variables.Load(new List<string> { "true" }, "keep_alive")[0]);

            ConfigurationSection rulesSection = configuration.GetSection("system.webServer/rewrite/rules");
            ConfigurationElementCollection rulesCollection = rulesSection.GetCollection();
            rulesCollection.Clear();
            if (variables.ContainsKey("rewrite"))
            {
                var rules = variables["rewrite"];
                for (int i = 0; i < rules.Count; i++)
                {
                    var rule = rules[i];
                    ConfigurationElement ruleElement = rulesCollection.CreateElement("rule");
                    ruleElement["name"] = @"rule" + i;
                    ruleElement["enabled"] = true;
                    ruleElement["patternSyntax"] = 0;
                    ruleElement["stopProcessing"] = false;

                    var parts = rule.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    ConfigurationElement matchElement = ruleElement.GetChildElement("match");
                    matchElement["ignoreCase"] = parts[0].EndsWith("/i", StringComparison.Ordinal);
                    matchElement["url"] = parts[0].EndsWith("/i", StringComparison.Ordinal) ? parts[0].Substring(0, parts[0].Length - 2) : parts[0];

                    ConfigurationElement actionElement = ruleElement.GetChildElement("action");
                    actionElement["type"] = 2;
                    actionElement["url"] = parts[1];
                    actionElement["appendQueryString"] = true;
                    rulesCollection.Add(ruleElement);
                }

                variables.Remove("rewrite");
            }

            application.Extra = variables;
        }

        public async Task SaveAsync(Application application)
        {
            var variables = new SortedDictionary<string, List<string>>();
            foreach (var item in application.Extra)
            {
                variables.Add(item.Key, item.Value);
            }

            var vDir = application.VirtualDirectories[0];

            Configuration config = application.GetWebConfiguration();
            ConfigurationSection defaultDocumentSection = config.GetSection("system.webServer/defaultDocument");
            ConfigurationElementCollection filesCollection = defaultDocumentSection.GetCollection("files");
            ConfigurationSection httpLoggingSection = application.Server.GetApplicationHostConfiguration().GetSection("system.webServer/httpLogging", application.Location);
            ConfigurationSection ipSecuritySection = application.Server.GetApplicationHostConfiguration().GetSection("system.webServer/security/ipSecurity", application.Location);

            ConfigurationSection requestFilteringSection = config.GetSection("system.webServer/security/requestFiltering");
            ConfigurationElement hiddenSegmentsElement = requestFilteringSection.GetChildElement("hiddenSegments");
            ConfigurationElementCollection hiddenSegmentsCollection = hiddenSegmentsElement.GetCollection();

            ConfigurationSection httpErrorsSection = config.GetSection("system.webServer/httpErrors");
            ConfigurationElementCollection httpErrorsCollection = httpErrorsSection.GetCollection();

            var urlCompressionSection = config.GetSection("system.webServer/urlCompression");

            ConfigurationSection httpProtocolSection = config.GetSection("system.webServer/httpProtocol");

            ConfigurationSection rewriteSection = config.GetSection("system.webServer/rewrite/rules");
            ConfigurationElementCollection rewriteCollection = rewriteSection.GetCollection();

            variables.Add("usehttps", new List<string> { (application.Site.Bindings[0].Protocol == "https").ToString() });
            variables.Add("addr", new List<string> { application.Site.Bindings[0].EndPoint.Address.ToString() });
            variables.Add("port", new List<string> { application.Site.Bindings[0].EndPoint.Port.ToString() });
            variables.Add("hosts", new List<string> { application.Site.Bindings[0].Host });
            variables.Add("root", new List<string> {$"{vDir.Path} {vDir.PhysicalPath}"});
            variables.Add("nolog", new List<string> { httpLoggingSection["dontLog"].ToString() });
            variables.Add("keep_alive", new List<string> { httpProtocolSection["allowKeepAlive"].ToString() });

            var indexes = StringExtensions.Combine(filesCollection.Select(item => item.RawAttributes["value"]), ",");
            variables.Add("indexes", new List<string> { indexes });

            var allows = new List<string>();
            var denys = new List<string>();
            foreach (ConfigurationElement item in ipSecuritySection.GetCollection())
            {
                string element = string.IsNullOrEmpty((string)item["subnetMask"])
                    ? (string)item["ipAddress"]
                    : $"{item["ipAddress"]}/{item["subnetMask"]}";
                if ((bool)item["allowed"])
                {
                    allows.Add(element);
                }
                else
                {
                    denys.Add(element);
                }
            }

            variables.Add("allowfrom", allows);
            variables.Add("denyfrom", denys);

            var segments = StringExtensions.Combine(hiddenSegmentsCollection.Select(item => item["segment"].ToString()), ",");
            variables.Add("denydirs", new List<string> { segments });

            foreach (ConfigurationElement item in httpErrorsCollection)
            {
                if ((uint)item["statusCode"] == 404 && (int)item["subStatusCode"] == 0
                    && (string)item["prefixLanguageFilePath"] == @"%SystemDrive%\inetpub\custerr"
                    && (long)item["responseMode"] == 1)
                {
                    variables.Add("nofile", new List<string> { item["path"].ToString() });
                }
            }

            variables.Add("usegzip", new List<string> { urlCompressionSection["doStaticCompression"].ToString() });

            var rules = new List<string>();
            foreach (ConfigurationElement item in rewriteCollection)
            {
                var action = item.GetChildElement("action");
                var match = item.GetChildElement("match");
                if ((long)action["type"] == 2)
                {
                    rules.Add(string.Format("{0}{2} {1}", match["url"], action["url"], (bool)match["ignoreCase"] ? "/i" : string.Empty));
                }
            }

            variables.Add("rewrite", rules);

            if (string.IsNullOrEmpty(application.Server.HostName))
            {
                var rows = new List<string>();
                foreach (var item in variables)
                {
                    foreach (var line in item.Value)
                    {
                        rows.Add($"{item.Key}={line}");
                    }
                }

                var fileName = Path.Combine("siteconf", application.ToFileName());
                File.WriteAllLines(fileName, rows);
            }
            else
            {
                using (var client = GetClient())
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync($"api/site/{application.ToFileName()}", variables);
                    if (response.IsSuccessStatusCode)
                    {
                    }
                }
            }
        }
    }
}
