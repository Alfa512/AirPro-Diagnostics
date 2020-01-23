using System;

namespace AirPro.Logging.Interface
{
    public interface IRequestLogEntryDto
    {
        DateTimeOffset? ActionEndTime { get; set; }
        string ActionExceptionMessage { get; set; }
        string ActionExceptionStackTrace { get; set; }
        DateTimeOffset? ActionStartTime { get; set; }
        string RawUrl { get; set; }
        string RequestMethod { get; set; }
        DateTimeOffset? ResultEndTime { get; set; }
        string ResultExceptionMessage { get; set; }
        string ResultExceptionStackTrace { get; set; }
        DateTimeOffset? ResultStartTime { get; set; }
        string RouteAction { get; set; }
        string RouteArea { get; set; }
        string RouteController { get; set; }
        string RouteId { get; set; }
        string RouteUrl { get; set; }
        string UserAddress { get; set; }
        string UserAgent { get; set; }
        Guid? UserGuid { get; set; }
        string SessionId { get; set; }
    }
}