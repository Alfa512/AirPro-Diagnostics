using System;
using System.Diagnostics.CodeAnalysis;
using AirPro.Logging.Interface;

namespace AirPro.Site.Filters
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class RequestLogEntryDto : IRequestLogEntryDto
    {
        public Guid? UserGuid { get; set; }
        public string SessionId { get; set; }

        public string UserAddress { get; set; }
        public string UserAgent { get; set; }

        public string RawUrl { get; set; }
        public string RouteUrl { get; set; }
        public string RouteArea { get; set; }
        public string RouteController { get; set; }
        public string RouteAction { get; set; }
        public string RouteId { get; set; }
        public string RequestMethod { get; set; }

        public DateTimeOffset? ActionStartTime { get; set; }
        public DateTimeOffset? ActionEndTime { get; set; }
        public DateTimeOffset? ResultStartTime { get; set; }
        public DateTimeOffset? ResultEndTime { get; set; }

        public string ActionExceptionMessage { get; set; }
        public string ActionExceptionStackTrace { get; set; }

        public string ResultExceptionMessage { get; set; }
        public string ResultExceptionStackTrace { get; set; }
    }
}