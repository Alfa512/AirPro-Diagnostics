using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReleaseNoteDto
    {
        int ReleaseNoteId { get; set; }
        string Version { get; set; }
        string Summary { get; set; }
        string DevelopmentId { get; set; }
        string ReleaseNote { get; set; }
        string UpdatedBy { get; set; }
        DateTimeOffset UpdatedDt { get; }
        ICollection<Guid> ImpactedRoleGuids { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}
