using Microsoft.AspNetCore.Mvc;
using SmtpEmailDemo.Services;
using System.Threading.Tasks;

namespace SmtpEmailDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendAttachMail")]
        public async Task<IActionResult> SendAttachMail([FromForm] EmailRequest emailRequest) // Use [FromForm] to handle file uploads
        {
            Stream attachmentStream = null;
            if (emailRequest.Attachment != null)
            {
                attachmentStream = emailRequest.Attachment.OpenReadStream();
            }

            await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body, attachmentStream, emailRequest.Attachment?.FileName, emailRequest.Html);
            return Ok("Email sent successfully!");
        }
    }
}
