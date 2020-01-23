using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.VehicleMakes
{
    public class VehicleMakeViewModelProfile : Profile
    {
        public VehicleMakeViewModelProfile()
        {
            CreateMap<IVehicleMakeDto, VehicleMakeViewModel>()
                .ReverseMap();
        }
    }
}