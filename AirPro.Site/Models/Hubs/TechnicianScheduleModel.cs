using System;
using Newtonsoft.Json;

namespace AirPro.Site.Models.Hubs
{
    public class TechnicianScheduleModel : TechnicianBaseModel
    {
        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTimeOffset EndTime { get; set; }
    }
}