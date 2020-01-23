using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IAirProToolDto
    {
        int? ToolId { get; set; }
        Guid? ToolKey { get; set; }
        string ToolName { get; set; }

        string ToolPassword { get; set; }

        string AutoEnginuityNum { get; set; }
        string AutoEnginuityVersion { get; set; }
        string CarDaqNum { get; set; }
        string DGNum { get; set; }

        string TeamViewerId { get; set; }
        string TeamViewerPassword { get; set; }

        string WindowsVersion { get; set; }
        string TabletModel { get; set; }
        string HubModel { get; set; }

        bool IPV6DisabledInd { get; set; }
        bool OneDriveSyncEnabledInd { get; set; }
        bool UpdatesServiceInd { get; set; }
        bool MeteredConnectionInd { get; set; }
        bool SelfScanEnabledInd { get; set; }

        string OBD2YConnector { get; set; }
        string AELatestCode { get; set; }
        string ChargerStyle { get; set; }
        string TabletSerialNumber { get; set; }
        string WifiCard { get; set; }
        string WifiHardwareId { get; set; }
        DateTimeOffset? WifiDriverDate { get; set; }
        string WifiDriverVersion { get; set; }
        string WifiMacAddress { get; set; }
        string ImageVersion { get; set; }
        bool CellularActiveInd { get; set; }
        string CellularProvider { get; set; }
        string CellularIMEI { get; set; }
        string HondaVersion { get; set; }
        string FJDSVersion { get; set; }
        string TechstreamVersion { get; set; }
        int? J2534Brand { get; set; }
        int? J2534Model { get; set; }
        string J2534Serial { get; set; }
        ToolType Type { get; set; }
        string TypeAsString { get; }

        IEnumerable<IAirProToolSubscriptionDto> Subscriptions { get; set; }
        IEnumerable<IAirProToolDepositDto> Deposits { get; set; }

        IEnumerable<KeyValuePair<Guid, string>> ShopAssignments { get; set; }
        IEnumerable<Guid> AccountAssignments { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}