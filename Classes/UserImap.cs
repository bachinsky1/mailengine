using Limilabs.Mail;
using Limilabs.Client.IMAP;


namespace MailEngine.Controllers
{
    
    public class UserImap
    {
        public Imap imap = new Imap();
       
        public Imap Connect()
        {
            imap.Connect(UserSettings.ImapServer, UserSettings.ImapPort, UserSettings.ImapUseTls);
            imap.UseBestLogin(UserSettings.ImapUser, UserSettings.ImapPassword);
            return imap;
        }

        public void CLose()
        {
            imap.Close();
        }
    }
}
