using System.IO;

namespace AirPro.Messaging.Interface
{
    public interface INotificationMessageAttachment
    {
        string ContentBase64 { get; set; }
        string MimeType { get; set; }
        string Filename { get; set; }
    }
}