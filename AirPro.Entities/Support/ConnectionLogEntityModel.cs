using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Support
{
    [Table("ConnectionLogs", Schema = "Support")]
    public class ConnectionLogEntityModel : IConnectionLogEntityModel
    {
        [Key]
        public Guid ConnectionGuid { get; set; }

        public Guid UserGuid { get; set; }

        public string PageUrl { get; set; }

        public DateTimeOffset ConnectionStartDt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? ConnectionEndDt { get; set; }
    }
}