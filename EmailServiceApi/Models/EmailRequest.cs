namespace EmailServiceApi.Models
{
    public class EmailRequest
    {
        public string ToEmail { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
    }
}
