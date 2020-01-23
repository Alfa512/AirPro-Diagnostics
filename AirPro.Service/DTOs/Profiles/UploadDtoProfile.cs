using AirPro.Entities.Common;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class UploadDtoProfile : Profile
    {
        public UploadDtoProfile()
        {
            CreateMap<IUploadDto, UploadEntityModel>();
            CreateMap<UploadEntityModel, UploadDto>();
        }
    }
}
