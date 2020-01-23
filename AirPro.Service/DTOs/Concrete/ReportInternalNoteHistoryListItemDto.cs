using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportInternalNoteHistoryListItemDto : IReportInternalNoteHistoryListItemDto
    {
        public int RequestId { get; set; }
        public string TechnicianNotes { get; set; }
        public string UpdatedByUserDisplay { get; set; }
        public Guid UpdatedByUserGuid { get; set; }
        public DateTime UpdatedDt { get; set; }
    }
}