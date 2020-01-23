using System;
using AirPro.Reports.DataSources.Interface;

namespace AirPro.Reports.DataSources.Concrete
{
    public class RepairInvoiceDataSource : IRepairInvoiceDataSource
    {
        // Shop Info.
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public string ShopFax { get; set; }
        public string ShopAddress1 { get; set; }
        public string ShopAddress2 { get; set; }
        public string ShopCity { get; set; }
        public string ShopState { get; set; }
        public string ShopZip { get; set; }

        // Repair Info.
        public int RepairOrderID { get; set; }
        public string RepairShopReferenceNumber { get; set; }
        public string RepairInsuranceCompany { get; set; }
        public string RepairInsuranceClaimNumber { get; set; }
        public int RepairOdometer { get; set; }
        public bool RepairAirBagsDeployed { get; set; }

        // Vehicle Info.
        public string VehicleVIN { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleTransmission { get; set; }

        // Invoice Info.
        public string CustomerMemo { get; set; }
        public DateTime InvoicedDt { get; set; }
        public string InvoiceCurrency { get; set; }
    }
}