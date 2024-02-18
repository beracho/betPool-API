using budgetManager.Helpers.Smtp;

namespace budgetManager.Services.Interfaces
{
    public interface IMailService
    {
        Task<String> SendEmailAsync(MailRequest mailRequest);
    }
}
