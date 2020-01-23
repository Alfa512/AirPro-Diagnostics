using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Billing
{
    [Table("Currencies", Schema = "Billing")]
    public class CurrencyEntityModel
    {
        [Key]
        public int CurrencyId { get; set; }
        public string Name { get; set; }
    }
}