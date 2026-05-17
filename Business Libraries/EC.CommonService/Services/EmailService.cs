using CakeShop.Core.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace EC.CommonService.Services;

public class EmailService : IEmailService
{
    private readonly string _host;
    private readonly int    _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _displayName;

    public EmailService(IConfiguration config)
    {
        _host        = config["Email:Host"]        ?? "smtp.gmail.com";
        _port        = int.TryParse(config["Email:Port"], out var p) ? p : 587;
        _username    = config["Email:Username"]    ?? "";
        _password    = config["Email:Password"]    ?? "";
        _displayName = config["Email:DisplayName"] ?? "CakeShop";
    }

    public async Task SendAsync(string to, string subject, string body, bool isHtml = true)
    {
        if (string.IsNullOrWhiteSpace(_username) || string.IsNullOrWhiteSpace(_password))
            throw new InvalidOperationException("Email 設定未完成，請確認 Email:Username 與 Email:Password");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_displayName, _username));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder();
        if (isHtml) builder.HtmlBody = body;
        else        builder.TextBody = body;
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_username, _password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
