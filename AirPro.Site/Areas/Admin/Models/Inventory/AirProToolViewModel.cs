using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Inventory
{
    public class AirProToolViewModel
    {
        public int? ToolId { get; set; }
        [DisplayName("Tool Name")]
        public string ToolName { get; set; }
        [DisplayName("Took Key")]
        public Guid? ToolKey { get; set; }
        [DisplayName("Shop")]
        public Guid? ShopGuid { get; set; }
        [DisplayName("Shop Name")]
        public string ShopName { get; set; }
        [DisplayName("Password")]
        public string ToolPassword { get; set; }
        [DisplayName("Tool Type")]
        public ToolType ToolType { get; set; }
    
        [DisplayName("Self Scan Enabled")]
        public bool SelfScanEnabledInd { get; set; }

        public IEnumerable<KeyValuePair<string, string>> AccountsList { get; set; }
        public IEnumerable<Guid> AccountAssignments { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ShopsList { get; set; }
        public IEnumerable<Guid> ShopAssignments { get; set; }
        public Dictionary<int, string> J2534BrandList { get; set; }
        public Dictionary<int, string> J2534ModelList { get; set; }
        public Dictionary<ToolType, string> ToolTypeList { get; set; }

        public AirProToolTabletTabViewModel TabletTab { get; set; }
        public AirProToolHardwareTabViewModel HardwareTab { get; set; }
        public AirProToolSubscriptionsTabViewModel SubscriptionsTab { get; set; }
        public AirProToolDepositsTabViewModel DepositsTab { get; set; }
        public AirProToolJ2534TabViewModel J2534Tab { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }

    public class AirProToolTabletTabViewModel
    {
        [DisplayName("Team Viewer Id")]
        public string TeamViewerId { get; set; }
        [DisplayName("Team Viewer Password")]
        public string TeamViewerPassword { get; set; }
        [DisplayName("Windows Version")]
        public string WindowsVersion { get; set; }
        [DisplayName("Tablet Model")]
        public string TabletModel { get; set; }
        [DisplayName("IPV6 Disabled")]
        public bool IPV6DisabledInd { get; set; }
        [DisplayName("OneDrive Enabled")]
        public bool OneDriveSyncEnabledInd { get; set; }
        [DisplayName("Updates Service")]
        public bool UpdatesServiceInd { get; set; }
        [DisplayName("Metered Connection")]
        public bool MeteredConnectionInd { get; set; }

        [Display(Name = "Tablet Serial Number")]
        public string TabletSerialNumber { get; set; }
        [Display(Name = "Wifi Card")]
        public string WifiCard { get; set; }
        [Display(Name = "Wifi Hardware Id")]
        public string WifiHardwareId { get; set; }
        [Display(Name = "Wifi Driver Date")]
        public DateTimeOffset? WifiDriverDate { get; set; }
        [Display(Name = "Wifi DriverVersion")]
        public string WifiDriverVersion { get; set; }
        [Display(Name = "Wifi Mac Address")]
        public string WifiMacAddress { get; set; }
        [Display(Name = "Image Version")]
        public string ImageVersion { get; set; }
        [Display(Name = "Cellular Active")]
        public bool CellularActiveInd { get; set; }
        [Display(Name = "Cellular Provider")]
        public string CellularProvider { get; set; }
        [Display(Name = "Cellular IMEI")]
        public string CellularIMEI { get; set; }
    }

    public class AirProToolHardwareTabViewModel
    {
        [DisplayName("Auto Enginuity Num")]
        public string AutoEnginuityNum { get; set; }
        [DisplayName("Auto Enginuity Version")]
        public string AutoEnginuityVersion { get; set; }
        [DisplayName("Car Daq Num")]
        public string CarDaqNum { get; set; }
        [DisplayName("DG Num")]
        public string DGNum { get; set; }
        [DisplayName("Hub Model")]
        public string HubModel { get; set; }

        [Display(Name = "OBD2 Connector")]
        public string OBD2YConnector { get; set; }
        [Display(Name="AE Latest Code")]
        public string AELatestCode { get; set; }
        [Display(Name = "Charger Style")]
        public string ChargerStyle { get; set; }
    }

    public class AirProToolSubscriptionsTabViewModel
    {
        [DisplayName("Honda Version")]
        public string HondaVersion { get; set; }
        [DisplayName("FJDS Version")]
        public string FJDSVersion { get; set; }
        [DisplayName("Techstream Version")]
        public string TechstreamVersion { get; set; }

        public IEnumerable<AirProToolSubscriptionViewModel> Subscriptions { get; set; }
    }

    public class AirProToolDepositsTabViewModel
    {
        public IEnumerable<AirProToolDepositViewModel> Deposits { get; set; }
    }

    public class AirProToolJ2534TabViewModel
    {

        [Display(Name = " Brand")]
        public int? J2534Brand { get; set; }
        [Display(Name = "Model")]
        public int? J2534Model { get; set; }
        [Display(Name = "Serial")]
        public string J2534Serial { get; set; }

    }
}