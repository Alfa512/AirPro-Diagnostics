using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("GroupsArchive", Schema = "Access")]
    public class GroupArchiveEntityModel : IArchiveEntityModel, IGroupEntityModel, IAuditBaseEntityModel
    {
        [Key]
        public int ArchiveId { get; set; }
        public DateTimeOffset ArchiveDt { get; set; }
        public string Description { get; set; }
        public Guid GroupGuid { get; set; }
        public string Name { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        public DateTimeOffset CreatedDt { get; set; }
        public Guid? UpdatedByUserGuid { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; }
    }
}