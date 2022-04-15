namespace MailEngine.Controllers
{
    public static class UserSettings
    {
        public static string ImapServer = "imap.gmail.com";
        public static string SmtpServer = "smtp.gmail.com";
        public static int ImapPort = 993;
        public static bool ImapUseTls = true;
        public static string ImapUser = "your_mail@gmail.com";
        public static string ImapPassword = "your_password";

        public static string SendGridKey = "Your key";
    }
}
