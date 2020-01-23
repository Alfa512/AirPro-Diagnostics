using System;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IUploadDto
    {
        int? UploadId { get; set; }

        string UploadKey { get; set; }
        UploadType UploadTypeId { get; set; }

        string UploadStorageName { get; set; }
        long UploadFileSizeBytes { get; set; }

        string UploadFileName { get; set; }
        string UploadFileExtension { get; set; }
        string UploadMimeType { get; set; }

        DateTime UploadedDateTime { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}
