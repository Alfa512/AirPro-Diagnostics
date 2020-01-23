using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Billing
{
    [Table("Cycles", Schema = "Billing")]
    public class BillingCycleEntityModel
    {
        [Key]
        public int CycleId { get; set; }
        public string CycleName { get; set; }
        public int SortOrder { get; set; }
    }
}
