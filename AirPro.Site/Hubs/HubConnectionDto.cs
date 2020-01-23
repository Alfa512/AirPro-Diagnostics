using System;

namespace AirPro.Site.Hubs
{
    public class HubConnectionDto
    {
        public Guid ConnectionGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string PageUrl { get; set; }
    }
}