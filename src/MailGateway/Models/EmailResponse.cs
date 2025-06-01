namespace MailGateway.Models
{
    public class EmailResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? MessageId { get; set; }
    }
}
