using System;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class UploadDto : IUploadDto
    {
        public int? UploadId { get; set; }
        public string UploadKey { get; set; }
        public UploadType UploadTypeId { get; set; }
        public string UploadStorageName { get; set; }
        public long UploadFileSizeBytes { get; set; }
        public string UploadFileName { get; set; }
        public string UploadFileExtension { get; set; }
        public string UploadMimeType { get; set; }
        public DateTime UploadedDateTime { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}
