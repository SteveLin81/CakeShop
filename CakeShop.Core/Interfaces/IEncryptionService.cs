namespace CakeShop.Core.Interfaces;

public interface IEncryptionService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
    string EncryptAesGcm(string plaintext);
    string DecryptAesGcm(string encryptedBase64);
}
