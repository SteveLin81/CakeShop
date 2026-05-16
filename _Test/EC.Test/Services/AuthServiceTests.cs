using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.CommonService.Services;
using EC.Entities.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EC.Test.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository>    _userRepoMock = new();
    private readonly Mock<IEncryptionService> _encryptionMock = new();
    private readonly Mock<IEmailService>      _emailMock = new();
    private readonly AuthService              _sut;

    public AuthServiceTests()
        => _sut = new AuthService(_userRepoMock.Object, _encryptionMock.Object,
                                   _emailMock.Object, NullLogger<AuthService>.Instance);

    // ── LoginAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsFailed()
    {
        _userRepoMock.Setup(r => r.GetByUsernameAsync("nobody"))
                     .ReturnsAsync((User?)null);

        var result = await _sut.LoginAsync(new LoginRequest { Username = "nobody", Password = "pw" });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("帳號或密碼錯誤");
        result.Token.Should().BeEmpty();
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ReturnsFailed()
    {
        var user = new User { Id = 1, Username = "test", PasswordHash = "hash" };
        _userRepoMock.Setup(r => r.GetByUsernameAsync("test")).ReturnsAsync(user);
        _encryptionMock.Setup(e => e.VerifyPassword("wrong", "hash")).Returns(false);

        var result = await _sut.LoginAsync(new LoginRequest { Username = "test", Password = "wrong" });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("帳號或密碼錯誤");
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsSuccess()
    {
        var user = new User { Id = 1, Username = "test", PasswordHash = "hash" };
        _userRepoMock.Setup(r => r.GetByUsernameAsync("test")).ReturnsAsync(user);
        _encryptionMock.Setup(e => e.VerifyPassword("pass", "hash")).Returns(true);
        _encryptionMock.Setup(e => e.EncryptAesGcm(It.IsAny<string>())).Returns("encrypted_token");

        var result = await _sut.LoginAsync(new LoginRequest { Username = "test", Password = "pass" });

        result.Success.Should().BeTrue();
        result.Message.Should().Be("登入成功");
        result.Username.Should().Be("test");
        result.Token.Should().Be("encrypted_token");
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_TokenPayloadContainsUserIdAndUsername()
    {
        var user = new User { Id = 42, Username = "alice", PasswordHash = "h" };
        _userRepoMock.Setup(r => r.GetByUsernameAsync("alice")).ReturnsAsync(user);
        _encryptionMock.Setup(e => e.VerifyPassword(It.IsAny<string>(), "h")).Returns(true);

        string capturedPayload = string.Empty;
        _encryptionMock.Setup(e => e.EncryptAesGcm(It.IsAny<string>()))
                       .Callback<string>(p => capturedPayload = p)
                       .Returns("token");

        await _sut.LoginAsync(new LoginRequest { Username = "alice", Password = "any" });

        capturedPayload.Should().StartWith("42|alice|");
    }

    // ── ValidateTokenAsync ───────────────────────────────────────────

    [Fact]
    public async Task ValidateTokenAsync_ValidToken_ReturnsTrue()
    {
        var future = DateTime.UtcNow.AddHours(1).ToString("O");
        _encryptionMock.Setup(e => e.DecryptAesGcm("tok"))
                       .Returns($"1|user|{future}");

        var result = await _sut.ValidateTokenAsync("tok");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateTokenAsync_ExpiredToken_ReturnsFalse()
    {
        // 使用 -2 天避免 DateTime.Parse 的本機時區轉換導致誤判
        var past = DateTime.UtcNow.AddDays(-2).ToString("O");
        _encryptionMock.Setup(e => e.DecryptAesGcm("expired"))
                       .Returns($"1|user|{past}");

        var result = await _sut.ValidateTokenAsync("expired");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateTokenAsync_InvalidFormat_ReturnsFalse()
    {
        _encryptionMock.Setup(e => e.DecryptAesGcm("bad")).Returns("only_one_part");

        var result = await _sut.ValidateTokenAsync("bad");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateTokenAsync_DecryptThrows_ReturnsFalse()
    {
        _encryptionMock.Setup(e => e.DecryptAesGcm(It.IsAny<string>()))
                       .Throws<Exception>();

        var result = await _sut.ValidateTokenAsync("garbage");

        result.Should().BeFalse();
    }

    // ── GetUsernameFromTokenAsync ─────────────────────────────────────

    [Fact]
    public async Task GetUsernameFromTokenAsync_ValidToken_ReturnsUsername()
    {
        var future = DateTime.UtcNow.AddHours(1).ToString("O");
        _encryptionMock.Setup(e => e.DecryptAesGcm("tok"))
                       .Returns($"1|alice|{future}");

        var username = await _sut.GetUsernameFromTokenAsync("tok");

        username.Should().Be("alice");
    }

    [Fact]
    public async Task GetUsernameFromTokenAsync_ExpiredToken_ReturnsNull()
    {
        var past = DateTime.UtcNow.AddDays(-2).ToString("O");
        _encryptionMock.Setup(e => e.DecryptAesGcm("expired"))
                       .Returns($"1|user|{past}");

        var username = await _sut.GetUsernameFromTokenAsync("expired");

        username.Should().BeNull();
    }

    [Fact]
    public async Task GetUsernameFromTokenAsync_DecryptThrows_ReturnsNull()
    {
        _encryptionMock.Setup(e => e.DecryptAesGcm(It.IsAny<string>()))
                       .Throws<Exception>();

        var username = await _sut.GetUsernameFromTokenAsync("garbage");

        username.Should().BeNull();
    }
}
