using System;
using AirPro.Common.Enumerations;

namespace AirPro.Entities.Inventory
{
    public interface IAirProTool
    {
        string AutoEnginuityNum { get; set; }
        string AutoEnginuityVersion { get; set; }
        string CarDaqNum { get; set; }
        string DGNum { get; set; }
        string HubModel { get; set; }
        bool IPV6DisabledInd { get; set; }
        bool MeteredConnectionInd { get; set; }
        bool OneDriveSyncEnabledInd { get; set; }
        bool SelfScanEnabledInd { get; set; }
        string TabletModel { get; set; }
        string TeamViewerId { get; set; }
        string TeamViewerPassword { get; set; }
        int ToolId { get; set; }
        Guid ToolKey { get; set; }
        string ToolPassword { get; set; }
        bool UpdatesServiceInd { get; set; }
        string WindowsVersion { get; set; }
        string OBD2YConnector { get; set; }
        string AELatestCode { get; set; }
        string ChargerStyle { get; set; }
        string TabletSerialNumber { get; set; }
        string WifiCard { get; set; }
        string WifiHardwareId { get; set; }
        DateTimeOffset? WifiDriverDate { get; set; }
        string WifiDriverVersion { get; set; }
        string ImageVersion { get; set; }
        string HondaVersion { get; set; }
        string FJDSVersion { get; set; }
        string TechstreamVersion { get; set; }
        bool CellularActiveInd { get; set; }
        string CellularProvider { get; set; }
        string CellularIMEI { get; set; }
        string WifiMacAddress { get; set; }
        int? J2534Brand { get; set; }
        int? J2534Model { get; set; }
        ToolType Type { get; set; }
        string J2534Serial { get; set; }
    }
}