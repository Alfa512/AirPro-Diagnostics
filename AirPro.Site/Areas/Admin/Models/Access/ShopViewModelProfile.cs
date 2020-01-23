using System;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.Inventory;
using AutoMapper;
using System.Linq;
using AirPro.Service.DTOs.Concrete;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class ShopViewModelProfile : Profile
    {
        public ShopViewModelProfile()
        {
            CreateMap<IShopDto, ShopViewModel>()
                .ForMember(d => d.AirProTools, opt => opt.MapFrom(s => s.AirProTools.Select(d => new AirProToolViewModel
                {
                    ToolId = d.ToolId,
                    ToolName = d.ToolName
                })))
                .ForMember(d => d.AccountAirProTools, opt => opt.MapFrom(s => s.AccountAirProTools.Select(d => new AirProToolViewModel
                {
                    ToolId = d.ToolId,
                    ToolName = d.ToolName
                })))
                .ForMember(d => d.ShopContacts, opt => opt.MapFrom(s => s.ShopContacts.Select(d => new ShopContactViewModel
                {
                    ShopContactGuid = d.ShopContactGuid,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    PhoneNumber = d.PhoneNumber,
                    HasRequests = d.HasRequests
                })));

            CreateMap<ShopViewModel, IShopDto>()
                .ForMember(d => d.AirProTools, opt => opt.Ignore())
                .ForMember(d => d.AccountAirProTools, opt => opt.Ignore());
        }
    }
}