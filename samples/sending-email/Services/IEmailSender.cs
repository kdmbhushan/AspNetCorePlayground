using SendingEmail.Models;

namespace SendingEmail.Services;

public interface IEmailSender
{
    Task SendEmailAsync(EmailViewModel model);
}
