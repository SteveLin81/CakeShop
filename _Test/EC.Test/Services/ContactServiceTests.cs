using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using EC.CommonService.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EC.Test.Services;

public class ContactServiceTests
{
    private static ContactService CreateSut(string adminTo = "")
    {
        var emailMock  = new Mock<IEmailService>();
        emailMock.Setup(e => e.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                 .Returns(Task.CompletedTask);

        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Email:AdminTo"]).Returns(adminTo);
        configMock.Setup(c => c["Email:Username"]).Returns("test@example.com");

        return new ContactService(emailMock.Object, configMock.Object, NullLogger<ContactService>.Instance);
    }

    [Fact]
    public async Task SubmitFormAsync_ValidForm_ReturnsSuccess()
    {
        var form = new ContactFormDto
        {
            Name    = "王小明",
            Email   = "test@example.com",
            Subject = "詢問商品",
            Message = "請問有無客製化服務？"
        };

        var result = await CreateSut("admin@test.com").SubmitFormAsync(form);

        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task SubmitFormAsync_ReturnsExpectedMessage()
    {
        var result = await CreateSut().SubmitFormAsync(new ContactFormDto());

        result.Message.Should().Be("感謝您的來信，我們將於 1-2 個工作天內回覆您！");
    }

    [Fact]
    public async Task SubmitFormAsync_EmptyForm_StillReturnsSuccess()
    {
        var result = await CreateSut().SubmitFormAsync(new ContactFormDto());

        result.Success.Should().BeTrue();
    }
}
