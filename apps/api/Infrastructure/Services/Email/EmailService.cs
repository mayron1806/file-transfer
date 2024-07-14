using Infrastructure.Settings;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Services.Email;

public class EmailService(SMTPSettings smtpSettings) : IEmailService
{
    private readonly SMTPSettings _smtpSettings = smtpSettings;

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _smtpSettings.FromName ?? _smtpSettings.From, // from name
                _smtpSettings.From // from email
            ));
            message.To.Add(new MailboxAddress("Destino", to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using(var client = new SmtpClient()) {
                client.ServerCertificateValidationCallback = (s, c , h ,e) => true;
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.EnableSSL);
                await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}