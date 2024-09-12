namespace SmtpEmailDemo.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, Stream attachment, string attachmentName, bool isHtml);
    }
}

