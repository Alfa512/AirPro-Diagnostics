using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Models.Repairs
{
    public class VehicleViewModelProfile : Profile
    {
        public VehicleViewModelProfile()
        {
            CreateMap<IVehicleDto, VehicleViewModel>();
        }
    }
}