using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Reminders.Application.Configurations;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Notification;

namespace Reminders.Infrastructure.Messaging;

public class EmailSender : IEmailSender
{
    private readonly SmtpYandexOptions _options;

    public EmailSender(IOptions<SmtpYandexOptions> options)
    {
        _options = options.Value;
    }
    
    public async Task SendEmailAsync(NotificationMessageDTO message)
    {
        var m = new MimeMessage();
        m.From.Add(new MailboxAddress("reminders service", _options.User));
        m.To.Add(new MailboxAddress("", message.ReceiverEmail));
        m.Subject = message.ReminderTitle;

        m.Body = new TextPart("html") { Text = $"notification from {message.NotificationId}" };

        using var client = new SmtpClient();

        await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.Auto);
        await client.AuthenticateAsync(_options.User, _options.Password);
        await client.SendAsync(m);
        await client.DisconnectAsync(true);
    }
}