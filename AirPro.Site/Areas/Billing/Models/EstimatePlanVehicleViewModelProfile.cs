using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Billing.Models
{
    public class EstimatePlanVehicleViewModelProfile : Profile
    {
        public EstimatePlanVehicleViewModelProfile()
        {
            CreateMap<IEstimatePlanVehicleDto, EstimatePlanVehicleViewModel>().ReverseMap();
        }
    }
}