namespace AirPro.Messaging.Interface
{
    public interface INotificationDestination
    {
        string Email { get; set; }
        string Text { get; set; }
        string TimeZoneInfoId { get; set; }
    }
}