using AirPro.Messaging.Interface;

namespace AirPro.Notifications.WebJob.Models
{
    internal class MessagingSettingsModel : IMessagingSettings
    {
        public string TwilioSid { get; set; }
        public string TwilioToken { get; set; }
        public string TwilioFromPhone { get; set; }
        public string SendGridAccount { get; set; }
        public string SendGridAccountKey { get; set; }
        public string SendGridFromAddress { get; set; }
        public string SendGridFromName { get; set; }
    }
}
