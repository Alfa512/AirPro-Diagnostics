using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AirPro.Messaging.Interface
{
    public interface INotificationMessage
    {
        IEnumerable<INotificationMessageAttachment> Attachments { get; set; }
        IEnumerable<INotificationDestination> Destinations { get; set; }
        string EmailBody { get; set; }
        string EmailSubject { get; set; }
        string TextMessage { get; set; }
    }
}