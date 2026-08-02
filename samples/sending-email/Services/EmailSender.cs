using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendingEmail.Configuration;
using SendingEmail.Models;
using SendingEmail.Services;

namespace SendingEmail.Services;

public class EmailSender : IEmailSender
{
    private readonly SMTPSettings _smtpSettings;

    public EmailSender(IOptions<SMTPSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(EmailViewModel model)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
        message.To.Add(MailboxAddress.Parse(model.To));
        message.Subject = model.Subject;

        if (!string.IsNullOrWhiteSpace(model.CC))
        {
            message.Cc.Add(MailboxAddress.Parse(model.CC));
        }

        var bodyBuilder = new BodyBuilder
        {
            TextBody = model.Body
        };

        if (model.Attachment != null && model.Attachment.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            await model.Attachment.CopyToAsync(memoryStream);
            bodyBuilder.Attachments.Add(model.Attachment.FileName, memoryStream.ToArray());
        }

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
