
namespace MailEngine.Classes
{
    public class Attachment
    {
        public int? Size { get; set; }
        public string FileName { get; set; }
        public string SafeFileName { get; set; }
        public string ContentType { get; set; }
        public string ContentId { get; set; }

    }
}
