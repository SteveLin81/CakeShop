using EC.CommonService.Services;
using FluentAssertions;

namespace EC.Test.Services;

public class EncryptionServiceTests
{
    private readonly EncryptionService _sut = new();

    // ── HashPassword ─────────────────────────────────────────────────

    [Fact]
    public void HashPassword_ReturnsBase64String()
    {
        var hash = _sut.HashPassword("password123");

        hash.Should().NotBeNullOrEmpty();
        Convert.TryFromBase64String(hash, new byte[256], out _).Should().BeTrue();
    }

    [Fact]
    public void HashPassword_SameInput_ReturnsSameHash()
    {
        var hash1 = _sut.HashPassword("abc");
        var hash2 = _sut.HashPassword("abc");

        hash1.Should().Be(hash2);
    }

    [Fact]
    public void HashPassword_DifferentInputs_ReturnsDifferentHashes()
    {
        var hash1 = _sut.HashPassword("password1");
        var hash2 = _sut.HashPassword("password2");

        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void HashPassword_EmptyString_ReturnsConsistentHash()
    {
        var hash1 = _sut.HashPassword(string.Empty);
        var hash2 = _sut.HashPassword(string.Empty);

        hash1.Should().Be(hash2);
    }

    // ── VerifyPassword ───────────────────────────────────────────────

    [Fact]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        var hash = _sut.HashPassword("mySecret");

        _sut.VerifyPassword("mySecret", hash).Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WrongPassword_ReturnsFalse()
    {
        var hash = _sut.HashPassword("correctPassword");

        _sut.VerifyPassword("wrongPassword", hash).Should().BeFalse();
    }

    [Fact]
    public void VerifyPassword_EmptyPassword_ReturnsFalse_WhenHashedFromNonEmpty()
    {
        var hash = _sut.HashPassword("somePassword");

        _sut.VerifyPassword(string.Empty, hash).Should().BeFalse();
    }

    // ── EncryptAesGcm ────────────────────────────────────────────────

    [Fact]
    public void EncryptAesGcm_ReturnsBase64String()
    {
        var encrypted = _sut.EncryptAesGcm("hello world");

        encrypted.Should().NotBeNullOrEmpty();
        Convert.TryFromBase64String(encrypted, new byte[256], out _).Should().BeTrue();
    }

    [Fact]
    public void EncryptAesGcm_SameInput_ProducesDifferentCiphertexts()
    {
        // 每次加密使用隨機 nonce，結果應不同
        var enc1 = _sut.EncryptAesGcm("same plaintext");
        var enc2 = _sut.EncryptAesGcm("same plaintext");

        enc1.Should().NotBe(enc2);
    }

    [Fact]
    public void EncryptAesGcm_MinimumLength_AtLeast28Bytes()
    {
        var encrypted = _sut.EncryptAesGcm("x");
        var bytes = Convert.FromBase64String(encrypted);

        // nonce(12) + tag(16) + at least 1 byte ciphertext
        bytes.Length.Should().BeGreaterThanOrEqualTo(29);
    }

    // ── DecryptAesGcm ────────────────────────────────────────────────

    [Theory]
    [InlineData("Hello, World!")]
    [InlineData("1|testuser|2099-01-01T00:00:00Z")]
    [InlineData("CakeShop甜蜜烘焙坊")]
    [InlineData("")]
    public void DecryptAesGcm_RoundTrip_ReturnsOriginalPlaintext(string plaintext)
    {
        var encrypted = _sut.EncryptAesGcm(plaintext);
        var decrypted = _sut.DecryptAesGcm(encrypted);

        decrypted.Should().Be(plaintext);
    }

    [Fact]
    public void DecryptAesGcm_TamperedData_ThrowsException()
    {
        var encrypted = _sut.EncryptAesGcm("test");
        var bytes = Convert.FromBase64String(encrypted);
        bytes[^1] ^= 0xFF; // 竄改最後一個 byte
        var tampered = Convert.ToBase64String(bytes);

        var act = () => _sut.DecryptAesGcm(tampered);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void DecryptAesGcm_InvalidBase64_ThrowsException()
    {
        var act = () => _sut.DecryptAesGcm("not-valid-base64!!!");

        act.Should().Throw<Exception>();
    }
}
