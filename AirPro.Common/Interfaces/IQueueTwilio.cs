namespace AirPro.Common.Interfaces
{
    public interface IQueueTwilio
    {
        string AccountSid { get; set; }
        string ApiVersion { get; set; }
        string Body { get; set; }
        string From { get; set; }
        string FromCity { get; set; }
        string FromCountry { get; set; }
        string FromState { get; set; }
        string FromZip { get; set; }
        string MessageSid { get; set; }
        string NumMedia { get; set; }
        string NumSegments { get; set; }
        string SmsMessageSid { get; set; }
        string SmsSid { get; set; }
        string SmsStatus { get; set; }
        string To { get; set; }
        string ToCity { get; set; }
        string ToCountry { get; set; }
        string ToState { get; set; }
        string ToZip { get; set; }
    }
}