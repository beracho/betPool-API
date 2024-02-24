using budgetManager.Helpers.Smtp;
using budgetManager.Repositories.Interfaces;
using budgetManager.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Drawing;

namespace budgetManager.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _config;
        private readonly IHtmlTemplateRepository _htmlTemplateRepo;
        private readonly IUserRepository _userRepo;
        public MailService(IOptions<MailSettings> mailSettings, IConfiguration config, IHtmlTemplateRepository htmlTemplateRepo, IUserRepository userRepo)
        {
            _mailSettings = mailSettings.Value;
            _config = config;
            _htmlTemplateRepo = htmlTemplateRepo;
            _userRepo = userRepo;
        }

        public async Task<string> GetEmailBody(string bodyType, string userEmail)
        {
            // Get body html template from database
            string emailBody = (await _htmlTemplateRepo.GetTemplateByName(bodyType)).Body;
            if (emailBody == null)
                return "";

            // get user information for emails
            var userFromRepo = await _userRepo.GetUserByUsernameOrEmail(userEmail);

            // If it is a recovery email get url information and replace in template
            userFromRepo.RecoveryKey = _userRepo.GenerateRecoveryKey();
            userFromRepo.RecoveryDate = DateTime.Now.AddDays(1);

            var updatedUser = await _userRepo.UpdateUser(userFromRepo);

            // replace information in html template
            emailBody = emailBody
            .Replace("[Username]", userFromRepo.Username)
            .Replace("[UrlKey]", $"{_config.GetSection("HostSettings:WebSiteUrl").Value}/resetPassword/{userFromRepo.RecoveryKey}");
            
            // Return string generated
            return emailBody;
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

                var hostName = _config["MailSettings:Host"];
                var portNumber = Int32.TryParse(_config["MailSettings:Port"], out int tempVal) ? tempVal : 0;
                smtp.Connect(hostName, portNumber, SecureSocketOptions.Auto);
                //smtp.Connect(_config["MailSettings:Host"], Int32.TryParse(_config["MailSettings:Port"], out int tempVal) ? tempVal : 0, SecureSocketOptions.Auto);
                smtp.Authenticate(_config["MailSettings:Mail"], _config["MailSettings:Password"]);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
