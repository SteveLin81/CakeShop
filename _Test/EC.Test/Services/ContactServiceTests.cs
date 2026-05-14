using CakeShop.Core.DTOs;
using EC.CommonService.Services;
using FluentAssertions;

namespace EC.Test.Services;

public class ContactServiceTests
{
    private readonly ContactService _sut = new();

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

        var result = await _sut.SubmitFormAsync(form);

        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task SubmitFormAsync_ReturnsExpectedMessage()
    {
        var result = await _sut.SubmitFormAsync(new ContactFormDto());

        result.Message.Should().Be("感謝您的來信，我們將於 1-2 個工作天內回覆您！");
    }

    [Fact]
    public async Task SubmitFormAsync_EmptyForm_StillReturnsSuccess()
    {
        var result = await _sut.SubmitFormAsync(new ContactFormDto());

        result.Success.Should().BeTrue();
    }
}
