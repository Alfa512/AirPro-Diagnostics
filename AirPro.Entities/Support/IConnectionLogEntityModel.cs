using System;

namespace AirPro.Entities.Support
{
    public interface IConnectionLogEntityModel
    {
        Guid ConnectionGuid { get; set; }
        Guid UserGuid { get; set; }
        string PageUrl { get; set; }
        DateTimeOffset ConnectionStartDt { get; set; }
    }
}