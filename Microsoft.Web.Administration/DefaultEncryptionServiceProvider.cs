namespace Microsoft.Web.Administration
{
    internal class DefaultEncryptionServiceProvider : IEncryptionServiceProvider
    {
        public string Decrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer)
        {
            return data;
        }

        public string Encrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer)
        {
            return data;
        }
    }
}
