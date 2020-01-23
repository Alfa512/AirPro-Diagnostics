using System;
using Newtonsoft.Json;

namespace AirPro.Site.Models.Hubs
{
    public class TechnicianBaseModel
    {
        [JsonProperty("userGuid")]
        public Guid UserGuid { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        [JsonProperty("userFullName")]
        public string UserFullName { get; set; }

        [JsonProperty("profileDisplayName")]
        public string ProfileDisplayName { get; set; }
    }
}