using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EC.CommonService.Services;

public class ContactService : IContactService
{
    private readonly IEmailService _email;
    private readonly ILogger<ContactService> _logger;
    private readonly string _adminTo;

    public ContactService(IEmailService email, IConfiguration config, ILogger<ContactService> logger)
    {
        _email   = email;
        _logger  = logger;
        _adminTo = config["Email:AdminTo"] ?? config["Email:Username"] ?? "";
    }

    public async Task<ContactFormResponse> SubmitFormAsync(ContactFormDto form)
    {
        if (!string.IsNullOrWhiteSpace(_adminTo))
        {
            try
            {
                var subject = $"[CakeShop 聯絡] {form.Subject}";
                var body = $"""
                    <div style="font-family:sans-serif;max-width:600px;margin:0 auto">
                      <h2 style="color:#2c3e50;border-bottom:2px solid #3498db;padding-bottom:8px">🎂 新客戶來信</h2>
                      <table style="width:100%;border-collapse:collapse">
                        <tr><td style="padding:8px;font-weight:bold;width:80px">姓名</td><td style="padding:8px">{System.Net.WebUtility.HtmlEncode(form.Name)}</td></tr>
                        <tr style="background:#f8f9fa"><td style="padding:8px;font-weight:bold">Email</td><td style="padding:8px">{System.Net.WebUtility.HtmlEncode(form.Email)}</td></tr>
                        <tr><td style="padding:8px;font-weight:bold">主旨</td><td style="padding:8px">{System.Net.WebUtility.HtmlEncode(form.Subject)}</td></tr>
                      </table>
                      <h3 style="color:#2c3e50;margin-top:16px">內容</h3>
                      <div style="background:#f8f9fa;padding:16px;border-radius:8px;white-space:pre-wrap">{System.Net.WebUtility.HtmlEncode(form.Message)}</div>
                    </div>
                    """;
                await _email.SendAsync(_adminTo, subject, body);
                _logger.LogInformation("聯絡表單信件已寄送至 {AdminTo}，來自 {Name}", _adminTo, form.Name);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "聯絡表單寄信失敗，仍回傳成功給用戶");
            }
        }

        return new ContactFormResponse
        {
            Success = true,
            Message = "感謝您的來信，我們將於 1-2 個工作天內回覆您！"
        };
    }
}
