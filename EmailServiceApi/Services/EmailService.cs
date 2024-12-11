using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmailServiceApi.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public bool SendOtpEmail(string toEmail, string otp)
        {
            if (string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(otp))
            {
                _logger.LogError("Recipient email and OTP cannot be null or empty.");
                throw new ArgumentException("Recipient email and OTP cannot be null or empty.");
            }

            string smtpHost = "smtp.gmail.com"; // gpnummx2.saps.gov.za
            int smtpPort = 587;
            string fromEmail = "thabohappy55@gmail.com";
            string username = "thabohappy55@gmail.com";
            string password = "nggznmhelsezcbge";

            // Create MimeMessage
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("No Reply", fromEmail));
            message.To.Add(new MailboxAddress("Recipient", toEmail));
            message.Subject = "MySAPS: Password Reset";
            message.Body = new TextPart("plain")
            {
                Text = $"Your reset password OTP is: {otp}. " +
                       $"Validate OTP Link: https://mysaps.saps.gov.za/service-complaints/#/update-password/{toEmail}?otp={otp}"
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    // Optionally disable SSL validation (not recommended for production)
                    //client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                    client.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                    client.Authenticate(username, password);
                    client.Send(message);
                    client.Disconnect(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email to {toEmail}: {ex.Message}");
                return false;
            }
        }
    }
}
