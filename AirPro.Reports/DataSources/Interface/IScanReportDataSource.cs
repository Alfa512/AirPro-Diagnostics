using System;

namespace AirPro.Reports.DataSources.Interface
{
    public interface IScanReportDataSource
    {
        bool RepairAirBagsDeployed { get; set; }
        bool RepairDrivable { get; set; }
        string RepairCreatedBy { get; set; }
        int RepairID { get; set; }
        string RepairInsuranceCompany { get; set; }
        int RepairOdometer { get; set; }
        string RepairShopReferenceNumber { get; set; }
        string RepairStatus { get; set; }
        string ShopName { get; set; }
        string ShopAddress { get; set; }
        string ShopCity { get; set; }
        string ShopState { get; set; }
        string ShopZip { get; set; }
        DateTime ReportCompletedDt { get; set; }
        int ReportID { get; set; }
        string ReportTechnicianNotes { get; set; }
        string RequestCreatedBy { get; set; }
        int RequestID { get; set; }
        string RequestNotes { get; set; }
        string RequestOtherWarningInfo { get; set; }
        string RequestProblemDescription { get; set; }
        string RequestTypeOfScan { get; set; }
        string RequestWarningIndicators { get; set; }
        string RepairPointOfImpacts { get; set; }
        string TechnicianContact { get; set; }
        string TechnicianName { get; set; }
        string VehicleMake { get; set; }
        string VehicleModel { get; set; }
        string VehicleTransmission { get; set; }
        string VehicleVIN { get; set; }
        string VehicleYear { get; set; }
        bool CanceledInd { get; set; }
    }
}