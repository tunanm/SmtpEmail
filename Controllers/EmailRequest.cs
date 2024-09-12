using Microsoft.AspNetCore.Http;
namespace SmtpEmailDemo.Controllers
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IFormFile? Attachment { get; set; }
        public bool Html { get; set; }
    }

}
