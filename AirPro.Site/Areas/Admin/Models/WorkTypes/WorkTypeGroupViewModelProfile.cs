using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.WorkTypes
{
    public class WorkTypeGroupViewModelProfile : Profile
    {
        public WorkTypeGroupViewModelProfile()
        {
            CreateMap<IWorkTypeGroupDto, WorkTypeGroupViewModel>();
        }
    }
}