namespace Microsoft.Web.Administration
{
    internal class DefaultEncryptionServiceProvider : IEncryptionServiceProvider
    {
        public string Decrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer)
        {
            // IMPORTANT: return empty string when cannot decrypt to match IIS behavior.
            return string.Empty;
        }

        public string Encrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer)
        {
            throw new NotImplementedException();
        }
    }
}
