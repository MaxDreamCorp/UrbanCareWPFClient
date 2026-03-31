using System.Security.Cryptography;
using System.Text;

namespace UrbanCareClient.Application.Security
{
    public class SecureTokenStorage
    {
        private readonly string _appDataPath;
        private readonly string _tokenFilePath;

        public SecureTokenStorage()
        {
            _appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                nameof(UrbanCareClient));

            _tokenFilePath = Path.Combine(_appDataPath, "token.dat");

            DeleteToken();

            Directory.CreateDirectory(_appDataPath);
        }

        public void SaveToken(string token)
        {
            byte[] plainTokenBytes = Encoding.UTF8.GetBytes(token);

#pragma warning disable CA1416 // Проверка совместимости платформы
            byte[] encryptedTokenBytes = ProtectedData.Protect(plainTokenBytes, null, DataProtectionScope.CurrentUser);
#pragma warning restore CA1416 // Проверка совместимости платформы

            File.WriteAllBytes(_tokenFilePath, encryptedTokenBytes);
        }

        public string? LoadToken()
        {
            if (!File.Exists(_tokenFilePath))
                return null;

            try
            {
                byte[] encryptedBytes = File.ReadAllBytes(_tokenFilePath);

#pragma warning disable CA1416 // Проверка совместимости платформы
                byte[] plainBytes = ProtectedData.Unprotect(
                    encryptedBytes,
                    null,
                    DataProtectionScope.CurrentUser);
#pragma warning restore CA1416 // Проверка совместимости платформы

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (CryptographicException)
            {
                return null;
            }
            catch
            {
                return null;
            }
        }

        public void DeleteToken()
        {
            if (File.Exists(_tokenFilePath))
                File.Delete(_tokenFilePath);
        }

    }
}
