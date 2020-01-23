namespace AirPro.Reports.DataSources.Interface
{
    public interface IRepairEstimateDataSource
    {
        bool RepairAirBagsDeployed { get; set; }
        string RepairInsuranceCompany { get; set; }
        string RepairInsuranceClaimNumber { get; set; }
        int RepairOdometer { get; set; }
        int RepairOrderID { get; set; }
        string RepairShopReferenceNumber { get; set; }
        string ShopAddress1 { get; set; }
        string ShopAddress2 { get; set; }
        string ShopCity { get; set; }
        string ShopFax { get; set; }
        string ShopName { get; set; }
        string ShopPhone { get; set; }
        string ShopState { get; set; }
        string ShopZip { get; set; }
        string VehicleMake { get; set; }
        string VehicleModel { get; set; }
        string VehicleTransmission { get; set; }
        string VehicleVIN { get; set; }
        string VehicleYear { get; set; }
        string CurrencyName { get; set; }
    }
}