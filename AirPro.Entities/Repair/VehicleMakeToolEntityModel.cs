using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Repair
{
    [Table("VehicleMakeTools", Schema = "Repair")]
    public class VehicleMakeToolEntityModel
    {
        [Key]
        public int VehicleMakeToolId { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(VehicleMake))]
        public int VehicleMakeId { get; set; }
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }
    }
}
