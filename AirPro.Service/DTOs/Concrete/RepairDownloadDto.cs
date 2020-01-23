using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    internal class RepairDownloadDto : IRepairDownloadDto
    {
        public int RepairId { get; set; }
        public string DownloadId { get; set; }
        public string DownloadType { get; set; }
        public string DisplayName { get; set; }
        public int SortOrder { get; set; }
    }
}
