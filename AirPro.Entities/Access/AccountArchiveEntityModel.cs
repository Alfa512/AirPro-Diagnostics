using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;
using AirPro.Entities.Scan;

namespace AirPro.Entities.Access
{
    [Table("AccountsArchive", Schema = "Access")]
    public class AccountArchiveEntityModel : IArchiveEntityModel, IAccountEntityModel, IAuditBaseEntityModel
    {
        [Key]
        public int ArchiveId { get; set; }
        public DateTimeOffset ArchiveDt { get; set; }
        public Guid AccountGuid { get; set; }
        public bool ActiveInd { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int DiscountPercentage { get; set; }
        public string Fax { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int StateId { get; set; }
        public string Zip { get; set; }
        public Guid? EmployeeGuid { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        public DateTimeOffset CreatedDt { get; set; }
        public Guid? UpdatedByUserGuid { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; }
    }
}