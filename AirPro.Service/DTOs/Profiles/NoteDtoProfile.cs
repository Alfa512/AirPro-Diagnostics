using AirPro.Entities.Common;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class NoteDtoProfile : Profile
    {
        public NoteDtoProfile()
        {
            CreateMap<INoteDto, NoteEntityModel>();
            CreateMap<NoteEntityModel, NoteDto>();
        }
    }
}
