using System;
using Limilabs.Mail;
using Limilabs.Client.IMAP;
using System.Collections.Generic;
using Limilabs.Mail.Headers;
using Limilabs.Mail.MIME;
using System.Collections.ObjectModel;
using Limilabs.Mail.Appointments;
using Limilabs.Mail.BusinessCard;
using Limilabs.Mail.DKIM;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.IO;
using MailEngine.Classes;

namespace MailEngine
{
    public class Message
    {
        public long Uid { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Html { get; set; }
        public string Text { get; set; }
        public string Rtf { get; set; }
        public DateTime? Date { get; set; }
        public bool IsRtf { get; set; }
        public bool IsHtml { get; set; }
        public bool IsText { get; set; }
        public bool IsReply { get; set; }
        public bool IsForward { get; set; }
        public bool IsPartial { get; set; }
        public string PartialId { get; set; }
        public List<Attachment> Attachments { get; set; }

        public Message()
        {
            Attachments = new List<Attachment>();
        }

        public static Message PrepareMessage(IMail email, long uid)
        {
            Message msg = new Message();
            msg.Uid = uid;
            msg.Subject = email.Subject;
            msg.From = email.From.ToString();
            msg.To = email.To.ToString();
            msg.Date = email.Date;
            msg.Html = email.Html;
            msg.Text = email.Text;

            foreach (MimeData mime in email.Attachments)
            {
                // Debug.WriteLine(mime.FileName);
                Attachment attachment = new();
                attachment.FileName = mime.FileName;
                attachment.SafeFileName = mime.SafeFileName;
                attachment.Size = mime.Size;
                attachment.ContentType = mime.ContentType.ToString();
                attachment.ContentId = mime.ContentId;
                // Debug.WriteLine(mime.ContentId);
                msg.Attachments.Add(attachment);
            }
            return msg;
        }
    }

    

}
