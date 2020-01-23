using System;
using Newtonsoft.Json;

namespace AirPro.Site.Models.Hubs
{
    public class TechncianConnectionModel : TechnicianBaseModel
    {
        [JsonProperty("connectionGuid")]
        public Guid ConnectionGuid { get; set; }

        [JsonProperty("connectionStartDt")]
        public DateTimeOffset ConnectionStartDt { get; set; }
    }
}