
using Limilabs.Mail;
using Limilabs.Client.IMAP;
using System.Collections.Generic;

namespace MailEngine
{
    public class Folder
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public char? SeparatorCharacter { get; set; }
        public string[] SubFolders { get; set; }
        public bool CanSelect { get; set; }
        public bool hasChildren { get; set; }

        public List<FolderFlag> flags { get; set; }
         
    }
     


}
