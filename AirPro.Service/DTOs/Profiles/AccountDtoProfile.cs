using System.Linq;
using AirPro.Entities;
using AirPro.Entities.Access;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class AccountDtoProfile : Profile
    {
        public AccountDtoProfile()
        {
            EntityDbContext db = new EntityDbContext();

            CreateMap<AccountEntityModel, AccountDto>()
                .ForMember(d => d.State, opt => opt.MapFrom(s => s.State.Abbreviation))
                .ForMember(d => d.AccountRep, opt => opt.MapFrom(s => s.EmployeeGuid != null ? s.Employee.DisplayName : ""))
                .ForMember(d => d.Users, opt => opt.MapFrom(s => s.AccountUsers.Select(r => new UserDto
                 {
                     UserGuid = r.UserGuid,
                     FirstName = r.User.FirstName,
                     LastName = r.User.LastName,
                     Email = r.User.Email
                 })));

            CreateMap<IAccountDto, AccountEntityModel>()
                .ForMember(d => d.State, opt => opt.Ignore())
                .ForMember(d => d.StateId, opt => opt.MapFrom(s => db.States.FirstOrDefault(l => l.Abbreviation == s.State).StateId));
        }
    }
}
