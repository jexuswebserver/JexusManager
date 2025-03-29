using System;

namespace Microsoft.Web.Administration
{
    internal static class Provider
    {
        private static readonly IEncryptionServiceProvider _defaultProvider = new DefaultEncryptionServiceProvider();

        public static string Decrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer)
        {
            try
            {
                return _defaultProvider.Decrypt(data, keyContainerName, cspProviderName, sessionKey, useOAEP, useMachineContainer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Provider.Decrypt: {ex.Message}");
                // For security reasons, don't expose exception details in the returned value
                return string.Empty;
            }
        }

        public static string Encrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer)
        {
            try
            {
                return _defaultProvider.Encrypt(data, keyContainerName, cspProviderName, sessionKey, useOAEP, useMachineContainer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Provider.Encrypt: {ex.Message}");
                throw new ServerManagerException("Failed to encrypt data", ex);
            }
        }
    }
}