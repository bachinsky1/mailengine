using Microsoft.AspNetCore.Mvc;
using Limilabs.Client.IMAP; 
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.MIME; 
using Limilabs.Mail.Headers; 
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks; 

namespace MailEngine.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        

        [HttpGet("{folder}/{messageUid}/{fileName}")]
        public IActionResult GetAttachment(string folder, int messageUid, string fileName)
        {
            
            using (Imap imap = new())
            {
                HttpResponseMessage response = new(HttpStatusCode.OK);
                imap.Connect(UserSettings.ImapServer, UserSettings.ImapPort, UserSettings.ImapUseTls);
                imap.UseBestLogin(UserSettings.ImapUser, UserSettings.ImapPassword);
                foreach (FolderInfo folderInfo in imap.GetFolders())
                {
                    if (folder == folderInfo.ShortName)
                    {
                        imap.Select(folderInfo);
                        byte[] eml = imap.GetMessageByUID(messageUid); 
                        IMail email = new MailBuilder().CreateFromEml(eml);

                        foreach (MimeData mime in email.Attachments)
                        {
                            if(mime.SafeFileName == fileName)
                            {
                                Debug.WriteLine(fileName);
                                imap.Close();
                                return File(mime.Data, mime.ContentType.ToString(), mime.SafeFileName);
                            } 
                        }
                    }
                }
                 
                return null;
            }
        }

        [HttpPost("Send")]
        public async Task<HttpResponseMessage> SendMail([FromForm] string to, [FromForm] string subject, [FromForm] string body, [FromForm] int useGateway = 0)
        {
             
            MailBuilder builder = new MailBuilder();
     
            builder.From.Add(new MailBox(UserSettings.ImapUser));
            builder.To.Add(new MailBox(to));
            builder.Subject = subject;
            builder.Text = body;
            builder.Html = body;

            Debug.WriteLine(useGateway);
            if (useGateway == 0)
            {
                IMail email = builder.Create();
                using (Smtp smtp = new Smtp())
                {
                    smtp.Connect(UserSettings.SmtpServer);    // or ConnectSSL for SSL
                    smtp.UseBestLogin(UserSettings.ImapUser, UserSettings.ImapPassword); // remove if authentication is not needed

                    ISendMessageResult result = smtp.SendMessage(email);
                    smtp.Close();
                    if (result.Status == SendMessageStatus.Success)
                    {
                        Debug.WriteLine("Sent via Own SMTP");
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    } 
                }
                Debug.WriteLine("Sent Error");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                string apiKey = "Your send gri api key";
                SendGridClient client = new SendGridClient(apiKey);
                SendGridMessage msg = MailHelper.CreateSingleEmail(new EmailAddress(UserSettings.ImapUser), new EmailAddress(to), subject, body, body);
                Response response = await client.SendEmailAsync(msg);
                Debug.WriteLine("Sent via SendGrid");
                return new HttpResponseMessage(response.StatusCode);
            }
        }

        [HttpGet("delete/{folder}/{messageUid}")]
        public HttpResponseMessage Get(string folder, long messageUid)
        {
            Debug.WriteLine(folder, messageUid);
            UserImap userImap = new();
            Imap imap = userImap.Connect();
            foreach (FolderInfo folderInfo in imap.GetFolders())
            {
                if (folderInfo.CanSelect)
                {
                    if (folder == folderInfo.ShortName)
                    {
                        imap.Select(folderInfo);
                        imap.DeleteMessageByUID(messageUid);
                        userImap.CLose();
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                }
            }

            userImap.CLose();
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpGet("{folder}/{messageUid}")]
        public Message GetMail(string folder, int messageUid)
        {
            using (Imap imap = new())
            {
                imap.Connect(UserSettings.ImapServer, UserSettings.ImapPort, UserSettings.ImapUseTls);
                imap.UseBestLogin(UserSettings.ImapUser, UserSettings.ImapPassword);

                foreach (FolderInfo folderInfo in imap.GetFolders())
                {
                    if (folder == folderInfo.ShortName)
                    {
                        imap.Select(folderInfo);
                        byte[] eml = imap.GetMessageByUID(messageUid);
                        IMail email = new MailBuilder().CreateFromEml(eml);
                        return Message.PrepareMessage(email, messageUid);
                    }
                }

                return null;
            }
        }

    }
}
