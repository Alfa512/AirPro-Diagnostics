using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReportInternalNoteHistoryListItemDto
    {
        int RequestId { get; set; }
        string TechnicianNotes { get; set; }
        string UpdatedByUserDisplay { get; set; }
        Guid UpdatedByUserGuid { get; set; }
        DateTime UpdatedDt { get; set; }
    }
}