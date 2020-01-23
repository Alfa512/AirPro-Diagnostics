using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.ReleaseNote
{
    public class ReleaseNoteViewModel : IReleaseNoteDto
    {
        public int ReleaseNoteId { get; set; }
        [Required, Display(Name = "Version")]
        public string Version { get; set; }
        [Required, Display(Name = "Summary")]
        public string Summary { get; set; }
        [Required, Display(Name = "Development Id")]
        public string DevelopmentId { get; set; }
        [Required, Display(Name = "Release Note"), DataType(DataType.MultilineText)]
        public string ReleaseNote { get; set; }

        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDt { get; set; }

        [Display(Name = "Impacted Roles")]
        public ICollection<Guid> ImpactedRoleGuids { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}