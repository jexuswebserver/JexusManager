namespace Microsoft.Web.Administration
{
    public interface IEncryptionServiceProvider
    {
        string Decrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer);
        string Encrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer);
    }
}
