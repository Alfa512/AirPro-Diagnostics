using System;
using System.Net;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Concrete
{
    internal class VehicleLookupDto
    {
        public string VehicleVIN { get; set; }
        public VehicleLookupService Service { get; set; }
        public string RequestBaseURL { get; set; }
        public string RequestString { get; set; }
        public bool RequestSuccess { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
        public string RequestMessage { get; set; }
        public string ResponseContent { get; set; }
        public DateTimeOffset RequestDt { get; set; }
        public int VehicleLookupId { get; set; }
    }
}