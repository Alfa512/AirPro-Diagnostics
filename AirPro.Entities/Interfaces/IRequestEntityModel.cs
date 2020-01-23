using System;

namespace AirPro.Entities.Interfaces
{
    public interface IRequestEntityModel
    {
        int RequestId { get; set; }
        int OrderId { get; set; }
        string Contact { get; set; }
        Guid? ContactUserGuid { get; set; }
        string Notes { get; set; }
        string OtherWarningInfo { get; set; }
        string ProblemDescription { get; set; }
        int? ReportId { get; set; }
        int? RequestCategoryId { get; set; }
        int RequestTypeId { get; set; }
        bool SeatRemovedInd { get; set; }
        int? ToolId { get; set; }
        Guid? ShopContactGuid { get; set; }
    }
}