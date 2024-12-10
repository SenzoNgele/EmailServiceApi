using EmailServiceApi.Models;
using EmailServiceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailServiceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-otp")]
        public IActionResult SendOTPEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState); // Returns 422 if validation fails
            }

            try
            {
                if (_emailService.SendOtpEmail(request.ToEmail, request.Otp))
                {
                    return Ok(new { Message = "Email sent successfully.", request.Otp });
                }
                else
                {
                    return StatusCode(500, "Failed to send email.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
