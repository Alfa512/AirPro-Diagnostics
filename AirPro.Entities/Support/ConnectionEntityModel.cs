using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Support
{
    [Table("Connections", Schema = "Support")]
    public class ConnectionEntityModel : IConnectionLogEntityModel
    {
        [Key]
        public Guid ConnectionGuid { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }
        public virtual UserEntityModel User { get; set; }

        public string PageUrl { get; set; }

        public DateTimeOffset ConnectionStartDt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed), Index]
        public int PageUrlHash { get; private set; }
    }
}
