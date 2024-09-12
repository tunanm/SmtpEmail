using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SmtpEmailDemo.Models;

namespace SmtpEmailDemo.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body,
            Stream attachment = null, string attachmentName = null, bool isHtml = false)
        {
            try
            {
                var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),
                    //DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = _emailSettings.EnableSsl,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                mailMessage.To.Add(toEmail);

                if (attachment != null && attachmentName != null)
                {
                    attachment.Position = 0; // Ensure the stream position is set to the beginning
                    mailMessage.Attachments.Add(new Attachment(attachment, attachmentName));
                }

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpFailedRecipientException ex)
            {
                // Log or handle specific failed recipient exceptions
                throw new Exception("Failed to send email. Please check the recipient address and your SMTP configuration.", ex);
            }
            catch (SmtpException ex)
            {
                // Log or handle generic SMTP exceptions
                throw new Exception("SMTP error occurred while sending the email. Please check the SMTP server settings.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle other general exceptions
                throw new Exception("An error occurred while sending the email.", ex);
            }
        }
    }
}
