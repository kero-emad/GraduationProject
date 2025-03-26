namespace Graduation_Project.services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
