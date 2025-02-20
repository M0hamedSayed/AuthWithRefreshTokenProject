using Service.Helpers;

namespace Service.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailMessage message);
    }
}
