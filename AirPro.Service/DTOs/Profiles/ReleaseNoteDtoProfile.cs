using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Entities.Access;
using AirPro.Entities.Common;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class ReleaseNoteDtoProfile : Profile
    {
        public ReleaseNoteDtoProfile()
        {
            CreateMap<ReleaseNoteEntityModel, ReleaseNoteDto>();

            CreateMap<IReleaseNoteDto, ReleaseNoteEntityModel>()
                .ForMember(d => d.UpdatedDt, opt => opt.Ignore())
                .ForMember(d => d.ReleaseNoteRoles, opt => opt.MapFrom(src => src.ImpactedRoleGuids.Select(roleGuid => new ReleaseNoteRoleEntityModel { RoleGuid = roleGuid })));
        }
    }
}
