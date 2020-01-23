using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Technician
{
    [Table("Locations", Schema = "Technician")]
    public class LocationEntityModel
    {
        [Key]
        public int LocationId { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        [ForeignKey(nameof(TechnicianProfileEntityModel.LocationId))]
        public virtual ICollection<TechnicianProfileEntityModel> TechnicianProfiles { get; set; }
    }
}
