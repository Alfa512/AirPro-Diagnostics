using System;
using System.Collections.Generic;
using System.Linq;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Inventory
{
    public class AirProToolViewModelProfile : Profile
    {
        public AirProToolViewModelProfile()
        {
            CreateMap<IAirProToolDto, AirProToolViewModel>()
                .ForMember(m => m.ToolType, opt => opt.MapFrom(m => m.Type))

                .ForMember(d => d.HardwareTab, opt => opt.MapFrom(s => new AirProToolHardwareTabViewModel
                {
                    AutoEnginuityNum = s.AutoEnginuityNum,
                    AutoEnginuityVersion = s.AutoEnginuityVersion,
                    CarDaqNum = s.CarDaqNum,
                    DGNum = s.DGNum,
                    HubModel = s.HubModel,
                    AELatestCode = s.AELatestCode,
                    ChargerStyle = s.ChargerStyle,
                    OBD2YConnector = s.OBD2YConnector
                }))
                .ForMember(d => d.TabletTab, opt => opt.MapFrom(s => new AirProToolTabletTabViewModel
                {
                    IPV6DisabledInd = s.IPV6DisabledInd,
                    MeteredConnectionInd = s.MeteredConnectionInd,
                    OneDriveSyncEnabledInd = s.OneDriveSyncEnabledInd,
                    TabletModel = s.TabletModel,
                    TeamViewerId = s.TeamViewerId,
                    TeamViewerPassword = s.TeamViewerPassword,
                    UpdatesServiceInd = s.UpdatesServiceInd,
                    WindowsVersion = s.WindowsVersion,
                    CellularActiveInd = s.CellularActiveInd,
                    CellularIMEI = s.CellularIMEI,
                    CellularProvider = s.CellularProvider,
                    ImageVersion = s.ImageVersion,
                    TabletSerialNumber = s.TabletSerialNumber,
                    WifiCard = s.WifiCard,
                    WifiDriverDate = s.WifiDriverDate,
                    WifiDriverVersion = s.WifiDriverVersion,
                    WifiHardwareId = s.WifiHardwareId,
                    WifiMacAddress = s.WifiMacAddress
                }))
                .ForMember(d => d.SubscriptionsTab, opt => opt.MapFrom(s => new AirProToolSubscriptionsTabViewModel
                {
                    FJDSVersion = s.FJDSVersion,
                    HondaVersion = s.HondaVersion,
                    TechstreamVersion = s.TechstreamVersion,
                    Subscriptions = s.Subscriptions.Select(d => new AirProToolSubscriptionViewModel
                    {
                        ToolSubscriptionId = d.ToolSubscriptionId,
                        ToolId = d.ToolId,
                        Username = d.Username,
                        Password = d.Password,
                        Vendor = d.Vendor
                    })
                }))
                .ForMember(d => d.J2534Tab, opt => opt.MapFrom(s => new AirProToolJ2534TabViewModel()
                {
                    J2534Model = s.J2534Model,
                    J2534Brand = s.J2534Brand,
                    J2534Serial = s.J2534Serial
                }))
                .ForMember(d => d.DepositsTab, opt => opt.MapFrom(s => new AirProToolDepositsTabViewModel
                {
                    Deposits = s.Deposits.Select(d => new AirProToolDepositViewModel
                    {
                        ToolDepositId = d.ToolDepositId,
                        ToolId = d.ToolId,
                        Date = d.Date,
                        Description = d.Description,
                        Amount = d.Amount
                    })
                }))
                .ForMember(d => d.ShopAssignments,
                    opt => opt.MapFrom(s => s.ShopAssignments.Select(m => m.Key).AsEnumerable()));


            CreateMap<AirProToolViewModel, IAirProToolDto>()
                .ForMember(m => m.AutoEnginuityNum, opt => opt.MapFrom(m => m.HardwareTab.AutoEnginuityNum))
                .ForMember(m => m.AutoEnginuityVersion, opt => opt.MapFrom(m => m.HardwareTab.AutoEnginuityVersion))
                .ForMember(m => m.CarDaqNum, opt => opt.MapFrom(m => m.HardwareTab.CarDaqNum))
                .ForMember(m => m.DGNum, opt => opt.MapFrom(m => m.HardwareTab.DGNum))
                .ForMember(m => m.HubModel, opt => opt.MapFrom(m => m.HardwareTab.HubModel))
                .ForMember(m => m.AELatestCode, opt => opt.MapFrom(m => m.HardwareTab.AELatestCode))
                .ForMember(m => m.ChargerStyle, opt => opt.MapFrom(m => m.HardwareTab.ChargerStyle))
                .ForMember(m => m.OBD2YConnector, opt => opt.MapFrom(m => m.HardwareTab.OBD2YConnector))

                .ForMember(m => m.IPV6DisabledInd, opt => opt.MapFrom(m => m.TabletTab.IPV6DisabledInd))
                .ForMember(m => m.MeteredConnectionInd, opt => opt.MapFrom(m => m.TabletTab.MeteredConnectionInd))
                .ForMember(m => m.OneDriveSyncEnabledInd, opt => opt.MapFrom(m => m.TabletTab.OneDriveSyncEnabledInd))
                .ForMember(m => m.TabletModel, opt => opt.MapFrom(m => m.TabletTab.TabletModel))
                .ForMember(m => m.TeamViewerId, opt => opt.MapFrom(m => m.TabletTab.TeamViewerId))
                .ForMember(m => m.TeamViewerPassword, opt => opt.MapFrom(m => m.TabletTab.TeamViewerPassword))
                .ForMember(m => m.UpdatesServiceInd, opt => opt.MapFrom(m => m.TabletTab.UpdatesServiceInd))
                .ForMember(m => m.WindowsVersion, opt => opt.MapFrom(m => m.TabletTab.WindowsVersion))
                .ForMember(m => m.CellularActiveInd, opt => opt.MapFrom(m => m.TabletTab.CellularActiveInd))
                .ForMember(m => m.CellularIMEI, opt => opt.MapFrom(m => m.TabletTab.CellularIMEI))
                .ForMember(m => m.CellularProvider, opt => opt.MapFrom(m => m.TabletTab.CellularProvider))
                .ForMember(m => m.ImageVersion, opt => opt.MapFrom(m => m.TabletTab.ImageVersion))
                .ForMember(m => m.TabletSerialNumber, opt => opt.MapFrom(m => m.TabletTab.TabletSerialNumber))
                .ForMember(m => m.WifiCard, opt => opt.MapFrom(m => m.TabletTab.WifiCard))
                .ForMember(m => m.WifiDriverDate, opt => opt.MapFrom(m => m.TabletTab.WifiDriverDate))
                .ForMember(m => m.WifiDriverVersion, opt => opt.MapFrom(m => m.TabletTab.WifiDriverVersion))
                .ForMember(m => m.WifiHardwareId, opt => opt.MapFrom(m => m.TabletTab.WifiHardwareId))
                .ForMember(m => m.WifiMacAddress, opt => opt.MapFrom(m => m.TabletTab.WifiMacAddress))

                .ForMember(m => m.FJDSVersion, opt => opt.MapFrom(m => m.SubscriptionsTab.FJDSVersion))
                .ForMember(m => m.HondaVersion, opt => opt.MapFrom(m => m.SubscriptionsTab.HondaVersion))
                .ForMember(m => m.TechstreamVersion, opt => opt.MapFrom(m => m.SubscriptionsTab.TechstreamVersion))

                .ForMember(m => m.J2534Model, opt => opt.MapFrom(m => m.J2534Tab.J2534Model))
                .ForMember(m => m.J2534Brand, opt => opt.MapFrom(m => m.J2534Tab.J2534Brand))
                .ForMember(m => m.J2534Serial, opt => opt.MapFrom(m => m.J2534Tab.J2534Serial))
                .ForMember(m => m.Type, opt => opt.MapFrom(m => m.ToolType))

                .ForMember(m => m.Subscriptions, opt => opt.MapFrom(m => m.SubscriptionsTab.Subscriptions.Select(d =>
                    new AirProToolSubscriptionDto
                    {
                        ToolSubscriptionId = d.ToolSubscriptionId,
                        ToolId = d.ToolId,
                        Username = d.Username,
                        Password = d.Password,
                        Vendor = d.Vendor
                    })))
                .ForMember(m => m.Deposits, opt => opt.MapFrom(m => m.DepositsTab.Deposits.Select(d =>
                    new AirProToolDepositDto
                    {
                        ToolDepositId = d.ToolDepositId,
                        ToolId = d.ToolId,
                        Date = d.Date,
                        Description = d.Description,
                        Amount = d.Amount,
                        DeleteInd = d.DeleteInd
                    })))

                .ForMember(d => d.ShopAssignments,
                    opt => opt.MapFrom(
                        s => s.ShopAssignments.Select(m => new KeyValuePair<Guid, string>(m, string.Empty))));


        }
    }
}