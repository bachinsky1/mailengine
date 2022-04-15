using Limilabs.Client.IMAP;
using Limilabs.Mail;
using Limilabs.Mail.MIME;
using MailEngine.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics; 

namespace MailEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FolderController : ControllerBase
    {
        
        [HttpGet]
        public IEnumerable<Folder> Get()
        {
            List<Folder> folders = new();

            UserImap userImap = new();
            Imap imap = userImap.Connect(); 

            foreach (FolderInfo folderInfo in imap.GetFolders())
            {
                if (folderInfo.CanSelect)
                {
                    imap.Select(folderInfo);
                    Folder folder = new();
                    folder.Name = folderInfo.Name;
                    folder.ShortName = folderInfo.ShortName;
                    folder.SeparatorCharacter = folderInfo.SeparatorCharacter;
                    folder.flags = folderInfo.Flags;

                    folders.Add(folder);
                }

            }
            userImap.CLose();
            return folders.ToArray();
        }

        [HttpGet("{folder}")]
        public IEnumerable<string> Get(string folder, string infoo = "")
        {
            string i = infoo;
            Debug.WriteLine(folder);
            UserImap userImap = new();
            Imap imap = userImap.Connect();
            int counter = 0;
            List<string> messages = new List<string>();

            foreach (FolderInfo folderInfo in imap.GetFolders())
            {
                if (folderInfo.CanSelect)
                {
                    if (folder == folderInfo.ShortName)
                    {
                        imap.Select(folderInfo);
                        List<long> uids = imap.GetAll();
                        uids.Sort();
                        uids.Reverse(); 

                        foreach (long uid in uids)
                        {
                            MessageInfo info = imap.GetMessageInfoByUID(uid); 
                            List<string> attachments = new();
                            messages.Add(info.UID.ToString());
                            Debug.WriteLine(info.UID.ToString());
                            if (counter++ > 8) break;
                        }
                    }
                }

            }

            userImap.CLose();
            return messages.ToArray();
        }


        [HttpGet("{folder}/{shift}")]
        public IEnumerable<Message> Get(string folder, int shift = 0)
        {
            if(shift < 0 )
            {
                shift = 0;
            }
            Debug.WriteLine("1", folder);
            UserImap userImap = new();
            Imap imap = userImap.Connect();
            int counter = 0;
            List<Message> messages = new List<Message>();

            foreach (FolderInfo folderInfo in imap.GetFolders())
            {
                if (folderInfo.CanSelect)
                {
                    if (folder == folderInfo.ShortName)
                    {
                        imap.Select(folderInfo);
                        List<long> uids = imap.GetAll();
                        uids.Sort();
                        uids.Reverse(); 
                        uids.RemoveRange(0, shift);

                        foreach (long uid in uids)
                        {
                            var eml = imap.GetMessageByUID(uid); 
                            IMail email = new MailBuilder().CreateFromEml(eml);
                            List<string> attachments = new();
                            messages.Add(Message.PrepareMessage(email, uid));

                            if (counter++ > 8) break;
                        }
                    }
                }

            }

            userImap.CLose();
            return messages.ToArray();
        }

        


    }
}
