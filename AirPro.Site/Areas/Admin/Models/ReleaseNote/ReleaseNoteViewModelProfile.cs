using System.Linq;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.ReleaseNote
{
    public class ReleaseNoteViewModelProfile : Profile
    {
        public ReleaseNoteViewModelProfile()
        {
            CreateMap<IReleaseNoteDto, ReleaseNoteViewModel>();

            CreateMap<ReleaseNoteViewModel, IReleaseNoteDto>();
        }
    }
}