using AirPro.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Access
{
    [Table("RegistrationAccounts", Schema = "Access")]
    public class RegistrationAccountEntityModel
    {
        [Key]
        public int RegistrationAccountId { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(15)]
        public string Phone { get; set; }
        [MaxLength(15)]
        public string Fax { get; set; }
        [MaxLength(1024)]
        public string Address1 { get; set; }
        [MaxLength(1024)]
        public string Address2 { get; set; }
        [MaxLength(1024)]
        public string City { get; set; }
        public string StateId { get; set; }
        [MaxLength(25)]
        public string Zip { get; set; }
        public int? DiscountPercentage { get; set; }
    }
}
