using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Models.Repairs
{
    public class RepairViewModelProfile : Profile
    {
        public RepairViewModelProfile()
        {
            CreateMap<RepairViewModel, IRepairDto>()
                .ReverseMap();
        }
    }
}