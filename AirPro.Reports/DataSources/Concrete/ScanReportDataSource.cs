using System;
using AirPro.Reports.DataSources.Interface;

namespace AirPro.Reports.DataSources.Concrete
{
    public class ScanReportDataSource : IScanReportDataSource
    {
        // Vehicle
        public string VehicleVIN { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleTransmission { get; set; } = "Unknown";

        // Shop
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopCity { get; set; }
        public string ShopState { get; set; }
        public string ShopZip { get; set; }

        // Repair
        public int RepairID { get; set; }
        public string RepairStatus { get; set; }
        public string RepairShopReferenceNumber { get; set; }
        public string RepairInsuranceCompany { get; set; }
        public int RepairOdometer { get; set; }
        public bool RepairAirBagsDeployed { get; set; }
        public bool RepairDrivable { get; set; }
        public string RepairCreatedBy { get; set; }
        public string RepairPointOfImpacts { get; set; }

        // Request
        public int RequestID { get; set; }
        public bool CanceledInd { get; set; }
        public string RequestTypeOfScan { get; set; }
        public string RequestWarningIndicators { get; set; } //(CheckEngine, ABS, Airbag, TPMS, Stability, Security, Other)
        public string RequestOtherWarningInfo { get; set; }
        public string RequestProblemDescription { get; set; } //Symptoms
        public string RequestNotes { get; set; }
        public string RequestCreatedBy { get; set; }

        // Technician Notes
        public int ReportID { get; set; }
        public string ReportTechnicianNotes { get; set; }
        public DateTime ReportCompletedDt { get; set; }

        // Technicial Contact.
        public string TechnicianName { get; set; }
        public string TechnicianContact { get; set; }
    }
}
