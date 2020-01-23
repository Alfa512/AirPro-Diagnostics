using System;

namespace AirPro.Entities.Interfaces
{
    public interface IAuditBaseEntityModel
    {
        Guid CreatedByUserGuid { get; set; }
        DateTimeOffset CreatedDt { get; set; }
        Guid? UpdatedByUserGuid { get; set; }
        DateTimeOffset? UpdatedDt { get; set; }
    }
}