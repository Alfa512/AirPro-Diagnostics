using System.Linq;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class GroupViewModelProfile : Profile
    {
        public GroupViewModelProfile()
        {
            CreateMap<IGroupDto, GroupViewModel>()
                .ForMember(d => d.RoleGuids, opt => opt.MapFrom(s => s.Roles.Select(r => r.Key)));
        }
    }
}