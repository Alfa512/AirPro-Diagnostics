using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Technician
{
    [Table("ProfileSchedules", Schema = "Technician")]
    public class ProfileScheduleEntityModel
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required, ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }

        public int DayOfWeek { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakStart { get; set; }

        public TimeSpan? BreakEnd { get; set; }

        public virtual UserEntityModel User { get; set; }
    }
}
