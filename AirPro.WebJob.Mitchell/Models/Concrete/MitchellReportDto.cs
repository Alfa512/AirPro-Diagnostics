using System;
using System.Collections.Generic;
using AirPro.WebJob.Mitchell.Models.Interface;
using Newtonsoft.Json;

namespace AirPro.WebJob.Mitchell.Models.Concrete
{
    [Serializable]
    internal class MitchellReportDto : IMitchellReportDto
    {
        private MitchellReportDto() { }

        public MitchellReportDto (IEnumerable<MitchellReportSystemDto> systems, MitchellReportVehicleDto vehicle, MitchellReportPdfDto pdf)
        {
            Systems = systems;
            Vehicle = vehicle;
            PDF = pdf;
        }

        public string Version { get; set; }
        public IEnumerable<IMitchellReportSystemDto> Systems { get; set; }
        public IMitchellReportVehicleDto Vehicle { get; set; }
        public IMitchellReportPdfDto PDF { get; set; }
        public string ScanMaker { get; set; }
        public string ScanTool { get; set; }
        [JsonConverter(typeof(ScanTimeStampConverter))]
        public DateTimeOffset ScanTimeStamp { get; set; }
        [JsonConverter(typeof(UploadTimeStampConverter))]
        public DateTimeOffset UploadTimeStamp { get; set; }
        [JsonConverter(typeof(LocalTimeStampConverter))]
        public DateTimeOffset LocalTimeStamp { get; set; }
        public string ScanToolId { get; set; }
        public string ScanReportId { get; set; }
        public string ScanDesignation { get; set; }
        public string ScanType { get; set; }
        public string ScanFullOrPartial { get; set; }
        public string RONumber { get; set; }
        public string RepairOrder { get; set; }
        public string PrePostScan { get; set; }
        public string ScanSubType { get; set; }
    }
}
