using System;
using System.Collections.Generic;
using System.Linq;
using AirPro.Entities.Access;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class GroupDtoProfile : Profile
    {
        public GroupDtoProfile()
        {
            CreateMap<GroupEntityModel, GroupDto>()
                .ForMember(d => d.Roles, opt =>
                {
                    opt.MapFrom(s =>
                        s.Roles.OrderBy(r => r.Role.Name)
                            .Select(r => new KeyValuePair<Guid, string>(r.RoleGuid, r.Role.Name)).ToList());
                }).ForMember(d => d.Users, opt =>
                {
                    opt.MapFrom(s =>
                        s.GroupUsers.Select(r => new UserDto
                        {
                            UserGuid = r.UserGuid,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName,
                            Email = r.User.Email
                        }));
                });

            CreateMap<IGroupDto, GroupEntityModel>()
                .ForMember(d => d.Roles,
                    opt => opt.MapFrom(
                        s => s.Roles.Select(
                            r => new GroupRoleEntityModel { GroupGuid = s.GroupGuid, RoleGuid = r.Key })));

        }
    }
}
