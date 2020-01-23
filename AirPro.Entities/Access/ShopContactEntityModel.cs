using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Access
{
    [Table("ShopContacts", Schema = "Access")]
    public class ShopContactEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ShopContactGuid { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(Shop))]
        public Guid ShopGuid { get; set; }
        public virtual ShopEntityModel Shop { get; set; }

        public bool DeletedInd { get; set; }
    }
}
