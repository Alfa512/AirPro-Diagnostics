using System;
using System.Collections.Generic;

namespace AirPro.WebJob.Mitchell.Models.Interface
{
    public interface IMitchellReportDto
    {
        string Version { get; set; }
        IEnumerable<IMitchellReportSystemDto> Systems { get; set; }
        IMitchellReportVehicleDto Vehicle { get; set; }
        IMitchellReportPdfDto PDF { get; set; }
        string ScanMaker { get; set; }
        string ScanTool { get; set; }
        DateTimeOffset ScanTimeStamp { get; set; }
        DateTimeOffset UploadTimeStamp { get; set; }
        DateTimeOffset LocalTimeStamp { get; set; }
        string ScanToolId { get; set; }
        string ScanReportId { get; set; }
        string ScanDesignation { get; set; }
        string ScanType { get; set; }
        string ScanFullOrPartial { get; set; }
        string RONumber { get; set; }
        string RepairOrder { get; set; }
        string PrePostScan { get; set; }
        string ScanSubType { get; set; }
    }
}
