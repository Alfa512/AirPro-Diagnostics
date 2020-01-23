using System.Linq;
using System.Web.Mvc;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.WorkTypes
{
    public class WorkTypeViewModelProfile : Profile
    {
        public WorkTypeViewModelProfile()
        {
            CreateMap<IWorkTypeDto, WorkTypeViewModel>()
                .ForMember(d => d.WorkTypeRequestTypeIds, opt => opt.MapFrom(s => s.RequestTypeSelection.Where(i => i.SelectedInd).Select(i => i.RequestTypeId).ToList()))
                .ForMember(d => d.WorkTypeRequestTypeSelectListItems, opt => opt.MapFrom(s => s.RequestTypeSelection.Select(i => new SelectListItem() { Selected = i.SelectedInd, Text = i.RequestTypeName, Value = i.RequestTypeId.ToString() })))
                .ForMember(d => d.WorkTypeVehicleMakeIds, opt => opt.MapFrom(s => s.VehicleMakeSelection.Where(i => i.SelectedInd).Select(i => i.VehicleMakeId).ToList()))
                .ForMember(d => d.WorkTypeVehicleMakeSelectListItems, opt => opt.MapFrom(s => s.VehicleMakeSelection.Select(i => new SelectListItem() { Selected = i.SelectedInd, Text = i.VehicleMakeName, Value = i.VehicleMakeId.ToString() })));

            CreateMap<WorkTypeViewModel, IWorkTypeDto>()
                .ForMember(d => d.VehicleMakeSelection, opt => opt.MapFrom(s => s.WorkTypeVehicleMakeIds.Select(i => new WorkTypeVehicleMakeViewModel { SelectedInd = true, VehicleMakeId = i })))
                .ForMember(d => d.RequestTypeSelection, opt => opt.MapFrom(s => s.WorkTypeRequestTypeIds.Select(i => new WorkTypeRequestTypeViewModel { SelectedInd = true, RequestTypeId = i })));
        }
    }
}