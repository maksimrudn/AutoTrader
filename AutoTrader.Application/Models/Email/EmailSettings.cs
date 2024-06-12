namespace AutoTrader.Application.Models.Email
{
    public class EmailSettings
    {
        public string NotificationEmail { get; set; } = string.Empty;
        public string NotificationEmailPassword { get; set; } = string.Empty;
        public string SMTPServer { get; set; } = string.Empty;
        public string SMTPPort { get; set; } = string.Empty;
    }
}
