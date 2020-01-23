using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class AirProToolDto : IAirProToolDto
    {
        public int? ToolId { get; set; }
        public Guid? ToolKey { get; set; }
        public string ToolName { get; set; }
        public string ToolPassword { get; set; }
        public string AutoEnginuityNum { get; set; }
        public string AutoEnginuityVersion { get; set; }
        public string CarDaqNum { get; set; }
        public string DGNum { get; set; }
        public string TeamViewerId { get; set; }
        public string TeamViewerPassword { get; set; }
        public string WindowsVersion { get; set; }
        public string TabletModel { get; set; }
        public string HubModel { get; set; }
        public bool IPV6DisabledInd { get; set; }
        public bool OneDriveSyncEnabledInd { get; set; }
        public bool UpdatesServiceInd { get; set; }
        public bool MeteredConnectionInd { get; set; }
        public bool SelfScanEnabledInd { get; set; }

        public string OBD2YConnector { get; set; }
        public string AELatestCode { get; set; }
        public string ChargerStyle { get; set; }
        public string TabletSerialNumber { get; set; }
        public string WifiCard { get; set; }
        public string WifiHardwareId { get; set; }
        public DateTimeOffset? WifiDriverDate { get; set; }
        public string WifiDriverVersion { get; set; }
        public string WifiMacAddress { get; set; }
        public string ImageVersion { get; set; }
        public bool CellularActiveInd { get; set; }
        public string CellularProvider { get; set; }
        public string CellularIMEI { get; set; }
        public string HondaVersion { get; set; }
        public string FJDSVersion { get; set; }
        public string TechstreamVersion { get; set; }

        public int? J2534Brand { get; set; }
        public int? J2534Model { get; set; }
        public string J2534Serial { get; set; }
        public ToolType Type { get; set; }
        public string TypeAsString => Type.ToString();

        public IEnumerable<IAirProToolSubscriptionDto> Subscriptions { get; set; }
        public IEnumerable<IAirProToolDepositDto> Deposits { get; set; }

        public IEnumerable<KeyValuePair<Guid, string>> ShopAssignments { get; set; }
        public IEnumerable<Guid> AccountAssignments { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}
