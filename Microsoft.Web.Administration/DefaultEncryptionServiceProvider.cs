using System;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Web.Administration
{
    internal class DefaultEncryptionServiceProvider : IEncryptionServiceProvider
    {
        /// <summary>
        /// Decrypts the given string using the Windows CNG API
        /// </summary>
        public string Decrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            try
            {
                // Decode base64 string first
                byte[] encryptedData = Convert.FromBase64String(data);

                // Get the key from the key container
                byte[] keyBlob = GetKeyFromContainer(keyContainerName, useMachineContainer);
                if (keyBlob == null || keyBlob.Length == 0)
                {
                    // Failed to get key
                    return string.Empty;
                }

                IntPtr algorithmHandle = IntPtr.Zero;
                IntPtr keyHandle = IntPtr.Zero;

                try
                {
                    // Open the algorithm provider
                    uint status = NativeMethods.BCryptOpenAlgorithmProvider(out algorithmHandle, NativeMethods.BCRYPT_RSA_ALGORITHM, null, 0);
                    if (status != 0)
                    {
                        // Failed to open algorithm provider
                        return string.Empty;
                    }

                    // Import the key
                    status = NativeMethods.BCryptImportKeyPair(algorithmHandle, IntPtr.Zero, NativeMethods.BCRYPT_RSAPRIVATE_BLOB, out keyHandle, keyBlob, keyBlob.Length, 0);
                    if (status != 0)
                    {
                        // Failed to import key
                        return string.Empty;
                    }

                    // Decrypt the data
                    uint paddingScheme = useOAEP ? NativeMethods.BCRYPT_PAD_OAEP : NativeMethods.BCRYPT_PAD_PKCS1;
                    byte[] decryptedBytes = null;
                    int resultLength = 0;

                    // First determine the output size
                    status = NativeMethods.BCryptDecrypt(keyHandle, encryptedData, encryptedData.Length, IntPtr.Zero, null, 0, null, 0, out resultLength, paddingScheme);
                    if (status != 0)
                    {
                        // Failed to determine output size
                        return string.Empty;
                    }

                    // Allocate buffer and perform actual decryption
                    decryptedBytes = new byte[resultLength];
                    status = NativeMethods.BCryptDecrypt(keyHandle, encryptedData, encryptedData.Length, IntPtr.Zero, null, 0, decryptedBytes, decryptedBytes.Length, out resultLength, paddingScheme);
                    if (status != 0)
                    {
                        // Failed to decrypt
                        return string.Empty;
                    }

                    // Trim to actual length and convert to string
                    return Encoding.Unicode.GetString(decryptedBytes, 0, resultLength);
                }
                finally
                {
                    // Clean up resources
                    if (keyHandle != IntPtr.Zero)
                        NativeMethods.BCryptDestroyKey(keyHandle);
                    if (algorithmHandle != IntPtr.Zero)
                        NativeMethods.BCryptCloseAlgorithmProvider(algorithmHandle, 0);
                }
            }
            catch (Exception)
            {
                // Return empty string in case of any exception to match IIS behavior
                return string.Empty;
            }
        }

        /// <summary>
        /// Encrypts the given string using the Windows CNG API
        /// </summary>
        public string Encrypt(string data, string keyContainerName, string cspProviderName, string sessionKey, bool useOAEP, bool useMachineContainer)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            try
            {
                // Get the key from the key container
                byte[] keyBlob = GetKeyFromContainer(keyContainerName, useMachineContainer);
                if (keyBlob == null || keyBlob.Length == 0)
                {
                    throw new CryptographicException("Failed to retrieve key from container");
                }

                IntPtr algorithmHandle = IntPtr.Zero;
                IntPtr keyHandle = IntPtr.Zero;

                try
                {
                    // Open the algorithm provider
                    uint status = NativeMethods.BCryptOpenAlgorithmProvider(out algorithmHandle, NativeMethods.BCRYPT_RSA_ALGORITHM, null, 0);
                    if (status != 0)
                    {
                        throw new CryptographicException($"Failed to open algorithm provider: {status}");
                    }

                    // Import the key
                    status = NativeMethods.BCryptImportKeyPair(algorithmHandle, IntPtr.Zero, NativeMethods.BCRYPT_RSAPRIVATE_BLOB, out keyHandle, keyBlob, keyBlob.Length, 0);
                    if (status != 0)
                    {
                        throw new CryptographicException($"Failed to import key: {status}");
                    }

                    // Convert string to bytes
                    byte[] dataBytes = Encoding.Unicode.GetBytes(data);

                    // Encrypt the data
                    uint paddingScheme = useOAEP ? NativeMethods.BCRYPT_PAD_OAEP : NativeMethods.BCRYPT_PAD_PKCS1;
                    int resultLength = 0;

                    // First determine the output size
                    status = NativeMethods.BCryptEncrypt(keyHandle, dataBytes, dataBytes.Length, IntPtr.Zero, null, 0, null, 0, out resultLength, paddingScheme);
                    if (status != 0)
                    {
                        throw new CryptographicException($"Failed to determine output size: {status}");
                    }

                    // Allocate buffer and perform actual encryption
                    byte[] encryptedBytes = new byte[resultLength];
                    status = NativeMethods.BCryptEncrypt(keyHandle, dataBytes, dataBytes.Length, IntPtr.Zero, null, 0, encryptedBytes, encryptedBytes.Length, out resultLength, paddingScheme);
                    if (status != 0)
                    {
                        throw new CryptographicException($"Failed to encrypt: {status}");
                    }

                    // Convert result to base64 string
                    return Convert.ToBase64String(encryptedBytes, 0, resultLength);
                }
                finally
                {
                    // Clean up resources
                    if (keyHandle != IntPtr.Zero)
                        NativeMethods.BCryptDestroyKey(keyHandle);
                    if (algorithmHandle != IntPtr.Zero)
                        NativeMethods.BCryptCloseAlgorithmProvider(algorithmHandle, 0);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException($"Encryption failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the RSA key blob from the specified container
        /// </summary>
        private byte[] GetKeyFromContainer(string keyContainerName, bool useMachineContainer)
        {
            IntPtr providerHandle = IntPtr.Zero;
            IntPtr keyHandle = IntPtr.Zero;

            try
            {
                // Open the key storage provider
                uint status = NativeMethods.NCryptOpenStorageProvider(out providerHandle, NativeMethods.MS_KEY_STORAGE_PROVIDER, 0);
                if (status != 0)
                    return null;

                // Open the key
                uint flags = useMachineContainer ? (uint)NativeMethods.NCRYPT_MACHINE_KEY_FLAG : 0;
                status = NativeMethods.NCryptOpenKey(providerHandle, out keyHandle, keyContainerName, 0, flags);
                if (status != 0)
                {
                    // Key not found or access denied
                    return null;
                }

                // Export the key
                int exportLength = 0;
                status = NativeMethods.NCryptExportKey(keyHandle, IntPtr.Zero, NativeMethods.BCRYPT_RSAPRIVATE_BLOB, IntPtr.Zero, null, 0, out exportLength, 0);
                if (status != 0)
                    return null;

                byte[] keyBlob = new byte[exportLength];
                status = NativeMethods.NCryptExportKey(keyHandle, IntPtr.Zero, NativeMethods.BCRYPT_RSAPRIVATE_BLOB, IntPtr.Zero, keyBlob, keyBlob.Length, out exportLength, 0);
                if (status != 0)
                    return null;

                return keyBlob;
            }
            finally
            {
                // Clean up resources
                if (keyHandle != IntPtr.Zero)
                    NativeMethods.NCryptFreeObject(keyHandle);
                if (providerHandle != IntPtr.Zero)
                    NativeMethods.NCryptCloseStorageProvider(providerHandle);
            }
        }
    }
}
