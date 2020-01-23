using System;
using System.Linq;
using AirPro.Entities.Scan;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class RequestDtoProfile : Profile
    {
        public RequestDtoProfile()
        {
            CreateMap<RequestEntityModel, IRequestDto>()
                .ForMember(d => d.RepairId, opt => opt.MapFrom(s => s.OrderId))
                .ForMember(d => d.RequestTypeName, opt => opt.MapFrom(s => s.RequestType.TypeName))
                .ForMember(d => d.VehicleMakeName, opt => opt.MapFrom(s => s.Repair.Vehicle.Make))
                .ForMember(d => d.VehicleModelName, opt => opt.MapFrom(s => s.Repair.Vehicle.Model))
                .ForMember(d => d.VehicleYear, opt => opt.MapFrom(s => s.Repair.Vehicle.Year))
                .ForMember(d => d.VehicleVIN, opt => opt.MapFrom(s => s.Repair.Vehicle.VehicleVIN))
                .ForMember(d => d.RequestCategoryName, opt => opt.MapFrom(s => s.RequestCategory.RequestCategoryName))
                .ForMember(d => d.ShopGuid, opt => opt.MapFrom(s => s.Repair.ShopGuid))
                .ForMember(d => d.ShopName, opt => opt.MapFrom(s => s.Repair.Shop.DisplayName))
                .ForMember(d => d.RequestCreateDt, opt => opt.MapFrom(s => s.CreatedDt.UtcDateTime))
                .ForMember(d => d.RequestCreateDtUtc, opt => opt.MapFrom(s => s.CreatedDt.UtcDateTime))
                .ForMember(d => d.TechnicianName, opt => opt.MapFrom(s => s.Report.ResponsibleTechnician.FirstName + " " + s.Report.ResponsibleTechnician.LastName ))
                .ForMember(d=> d.ScanUploadDt, opt=> opt.MapFrom(s => MapUpload(s)));
        }

        private DateTime? MapUpload(RequestEntityModel arg)
        {
            var lastScan = arg.ScanUploads.FirstOrDefault();

            return lastScan?.CreatedDt.UtcDateTime;
        }
    }
}