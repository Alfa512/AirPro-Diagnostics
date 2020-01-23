namespace AirPro.Service.DTOs.Interface
{
    public interface IRepairDownloadDto
    {
        int RepairId { get; set; }
        string DownloadId { get; set; }
        string DownloadType { get; set; }
        string DisplayName { get; set; }
        int SortOrder { get; set; }
    }
}