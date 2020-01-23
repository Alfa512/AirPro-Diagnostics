using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class ReleaseNoteDto : IReleaseNoteDto
    {
        public int ReleaseNoteId { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public string DevelopmentId { get; set; }
        public string ReleaseNote { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDt { get; set; }
        public ICollection<Guid> ImpactedRoleGuids { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}
