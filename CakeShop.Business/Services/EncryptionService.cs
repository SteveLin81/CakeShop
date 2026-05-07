using System.Security.Cryptography;
using System.Text;
using CakeShop.Core.Interfaces;

namespace CakeShop.Business.Services;

public class EncryptionService : IEncryptionService
{
    private const string MasterSecret = "CakeShopMasterSecret2024!#$%AES256GCM";
    private const string PasswordSalt = "CakeShopPasswordSalt@2024";

    private byte[] DeriveKey()
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(MasterSecret));
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var combined = password + PasswordSalt;
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string hash)
        => HashPassword(password) == hash;

    public string EncryptAesGcm(string plaintext)
    {
        var key = DeriveKey();
        var nonce = new byte[12];
        RandomNumberGenerator.Fill(nonce);

        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = new byte[plaintextBytes.Length];
        var tag = new byte[16];

        using var aesGcm = new AesGcm(key, 16);
        aesGcm.Encrypt(nonce, plaintextBytes, ciphertext, tag);

        // Layout: nonce(12) | tag(16) | ciphertext
        var result = new byte[28 + ciphertext.Length];
        Buffer.BlockCopy(nonce, 0, result, 0, 12);
        Buffer.BlockCopy(tag, 0, result, 12, 16);
        Buffer.BlockCopy(ciphertext, 0, result, 28, ciphertext.Length);

        return Convert.ToBase64String(result);
    }

    public string DecryptAesGcm(string encryptedBase64)
    {
        var key = DeriveKey();
        var data = Convert.FromBase64String(encryptedBase64);

        var nonce = new byte[12];
        var tag = new byte[16];
        var ciphertext = new byte[data.Length - 28];

        Buffer.BlockCopy(data, 0, nonce, 0, 12);
        Buffer.BlockCopy(data, 12, tag, 0, 16);
        Buffer.BlockCopy(data, 28, ciphertext, 0, ciphertext.Length);

        var plaintext = new byte[ciphertext.Length];
        using var aesGcm = new AesGcm(key, 16);
        aesGcm.Decrypt(nonce, ciphertext, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }
}
