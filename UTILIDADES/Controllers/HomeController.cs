using Microsoft.AspNetCore.Mvc;
using UTILIDADES.Interfaces;

namespace UTILIDADES.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-email")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendEmail(
            [FromForm] string to,
            [FromForm] string subject,
            [FromForm] string htmlBody,
            [FromForm] string? cc,
            [FromForm] string? bcc,
            [FromForm] IFormFile attachment)
        {
            if (attachment != null && attachment.Length > 0)
            {
                using (var stream = attachment.OpenReadStream())
                {
                    var response = await _emailService.SendEmailAsync(to, subject, htmlBody, cc, bcc, stream, attachment.FileName);
                    return Ok(new { Message = response });
                }
            }
            else
            {
                var response = await _emailService.SendEmailAsync(to, subject, htmlBody, cc, bcc, null, null);
                return Ok(new { Message = response });
            }
        }
    }

    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
