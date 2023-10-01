using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WebApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _config;

        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _config = emailConfig.Value;
        }


        public async Task SendEmailAsnyc(string bodyHtml, string subject, string fromName, string fromAddress)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(fromName, fromAddress));

            message.To.Add(new MailboxAddress(_config.ToName, _config.ToAddress));
            message.Cc.Add(new MailboxAddress(_config.CcName, _config.CcAddress));
            
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = bodyHtml
            };

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                if (_config.UseTls)
                {
                    await client.ConnectAsync(_config.SmtpServer, _config.SmtpTlsPort, SecureSocketOptions.StartTls);
                }
                else
                {
                    await client.ConnectAsync(_config.SmtpServer, _config.SmtpSslPort, SecureSocketOptions.SslOnConnect);
                }

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(_config.UserName, _config.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
