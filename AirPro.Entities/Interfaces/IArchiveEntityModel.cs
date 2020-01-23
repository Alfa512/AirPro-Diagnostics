using System;

namespace AirPro.Entities.Interfaces
{
    public interface IArchiveEntityModel
    {
        int ArchiveId { get; set; }
        DateTimeOffset ArchiveDt { get; set; }
    }
}