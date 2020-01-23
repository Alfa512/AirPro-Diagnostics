using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Technician
{
    [Table("Profiles", Schema = "Technician")]
    public class TechnicianProfileEntityModel : AuditBaseEntityModel
    {
        [Key, Required, ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(128)]
        public string DisplayName { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(128)]
        public string EmployeeId { get; set; }

        public string OtherNotes { get; set; }

        public int? LocationId { get; set; }
        [ForeignKey(nameof(LocationId))]
        public virtual LocationEntityModel Location { get; set; }

        [Required]
        public bool ActiveInd { get; set; } = true;

        public virtual UserEntityModel User { get; set; }

        [ForeignKey(nameof(ProfileVehicleMakeEntityModel.UserGuid))]
        public virtual ICollection<ProfileVehicleMakeEntityModel> VehicleMakes { get; set; }

        [ForeignKey(nameof(ProfileScheduleEntityModel.UserGuid))]
        public virtual ICollection<ProfileScheduleEntityModel> Schedules { get; set; }

        [ForeignKey(nameof(ProfileTimeOffEntryEntityModel.UserGuid))]
        public virtual ICollection<ProfileTimeOffEntryEntityModel> TimeOffEntries { get; set; }

    }
}
