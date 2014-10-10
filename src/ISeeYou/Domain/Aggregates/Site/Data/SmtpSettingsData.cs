namespace ISeeYou.Domain.Aggregates.Site.Data
{
    public class SmtpSettingsData
    {
        public string SmtpServer { get; set; }
        public int SmtpServerPort { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}