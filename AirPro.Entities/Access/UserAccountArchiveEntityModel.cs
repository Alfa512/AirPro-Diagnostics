using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("UserAccountsArchive", Schema = "Access")]
    public class UserAccountArchiveEntityModel : IArchiveEntityModel, IUserAccountEntityModel, IAuditBaseEntityModel
    {
        [Key]
        public int ArchiveId { get; set; }
        public DateTimeOffset ArchiveDt { get; set; }
        public Guid AccountGuid { get; set; }
        public Guid UserGuid { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        public DateTimeOffset CreatedDt { get; set; }
        public Guid? UpdatedByUserGuid { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; }
    }
}