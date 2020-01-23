using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Common;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("Accounts", Schema = "Access")]
    public class AccountEntityModel : AuditBaseEntityModel, IAccountEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountGuid { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual StateEntityModel State { get; set; }
        public string Zip { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }

        public int DiscountPercentage { get; set; }

        public bool ActiveInd { get; set; } = true;

        [ForeignKey(nameof(AccountGuid))]
        public virtual ICollection<ShopEntityModel> Shops { get; set; }

        [ForeignKey(nameof(AccountGuid))]
        public virtual ICollection<UserAccountEntityModel> AccountUsers { get; set; }

        [ForeignKey(nameof(Inventory.AirProToolAccountEntityModel.AccountGuid))]
        public virtual ICollection<Inventory.AirProToolAccountEntityModel> AirProTools { get; set; }

        [ForeignKey(nameof(Employee))]
        public Guid? EmployeeGuid { get; set; }
        public virtual UserEntityModel Employee { get; set; }
    }
}
