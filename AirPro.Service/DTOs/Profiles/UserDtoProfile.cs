using System.Linq;
using AirPro.Entities.Access;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class UserDtoProfile : Profile
    {
        public UserDtoProfile()
        {
            CreateMap<UserEntityModel, UserDto>()
                .ForMember(d => d.UserGuid, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.GroupMemberships, opt => opt.MapFrom(s => s.UserGroups.Select(g => g.GroupGuid)))
                .ForMember(d => d.AccountMemberships, opt => opt.MapFrom(s => s.UserAccounts.Select(a => a.AccountGuid)))
                .ForMember(d => d.ShopMemberships, opt => opt.MapFrom(s => s.UserShops.Select(p => p.ShopGuid)))
                .ForMember(d => d.EmployeeAssignedInd, opt => opt.MapFrom(s => s.EmployeeAccounts.Any(x => x.ActiveInd) || s.EmployeeShops.Any(x => x.ActiveInd)));

            CreateMap<IUserDto, UserEntityModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserGuid))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Email))
                .ForMember(d => d.EmailConfirmed, opt => opt.Ignore())
                .ForMember(d => d.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(d => d.AccessFailedCount, opt => opt.Ignore())
                .ForMember(d => d.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(d => d.UserGroups,
                    opt => opt.MapFrom(
                        s => s.GroupMemberships.Select(
                            p => new UserGroupEntityModel { UserGuid = s.UserGuid, GroupGuid = p })))
                .ForMember(d => d.UserAccounts,
                    opt => opt.MapFrom(
                        s => s.AccountMemberships.Select(
                            p => new UserAccountEntityModel { UserGuid = s.UserGuid, AccountGuid = p })))
                .ForMember(d => d.UserShops,
                    opt => opt.MapFrom(
                        s => s.ShopMemberships.Select(
                            p => new UserShopEntityModel() { UserGuid = s.UserGuid, ShopGuid = p })));
        }
    }
}
