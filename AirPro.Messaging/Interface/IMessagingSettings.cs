namespace AirPro.Messaging.Interface
{
    public interface IMessagingSettings
    {
        string TwilioSid { get; set; }
        string TwilioToken { get; set; }
        string TwilioFromPhone { get; set; }
        string SendGridAccount { get; set; }
        string SendGridAccountKey { get; set; }
        string SendGridFromAddress { get; set; }
        string SendGridFromName { get; set; }
    }
}