using budgetManager.Helpers.Smtp;
using budgetManager.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace budgetManager.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _config;
        public MailService(IOptions<MailSettings> mailSettings, IConfiguration config)
        {
            _mailSettings = mailSettings.Value;
            _config = config;
        }

        public async Task<String> SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_config["MailSettings:DisplayName"]);
                email.From.Add(MailboxAddress.Parse(_config["MailSettings:Mail"]));
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.CheckCertificateRevocation = false;

                smtp.Connect(_config["MailSettings:Host"], Int32.Parse(_config["MailSettings:Port"]), SecureSocketOptions.Auto);
                smtp.Authenticate(_config["MailSettings:Mail"], _config["MailSettings:Password"]);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                return "It works";
            }
            catch (Exception ex)
            {
                return "Failed email send";
            }
        }
    }
}
