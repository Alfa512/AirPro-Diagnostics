using System;
using System.Collections.Generic;
using System.Linq;
using AirPro.Entities.Inventory;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class AirProToolDtoProfile : Profile
    {
        public AirProToolDtoProfile()
        {
            CreateMap<AirProToolEntityModel, AirProToolDto>()
                .ForMember(m => m.Subscriptions, opt => opt.MapFrom(d => d.Subscriptions.Select(s =>
                    new AirProToolSubscriptionDto
                    {
                        ToolSubscriptionId = s.ToolSubscriptionId,
                        ToolId = s.ToolId,
                        Vendor = s.Vendor,
                        Username = s.Username,
                        Password = s.Password
                    })))
                .ForMember(m => m.Deposits, opt => opt.MapFrom(d => d.Deposits.Where(w => w.DeleteInd == false)
                    .Select(s =>
                        new AirProToolDepositDto
                        {
                            ToolDepositId = s.ToolDepositId,
                            ToolId = s.ToolId,
                            Date = s.Date,
                            Description = s.Description,
                            Amount = s.Amount
                        })))
                .ForMember(m => m.AccountAssignments, opt => opt.MapFrom(d => d.Accounts
                    .Select(s => s.AccountGuid).ToList()))
                .ForMember(m => m.ShopAssignments, opt => opt.MapFrom(d => d.Shops
                    .Select(s => new KeyValuePair<Guid, string>(s.ShopGuid, s.Shop.Name)).ToList()));

            CreateMap<IAirProToolDto, AirProToolEntityModel>()
                .ForMember(m => m.Subscriptions, opt => opt.MapFrom(d => d.Subscriptions.Select(s =>
                    new AirProToolSubscriptionEntityModel
                    {
                        ToolSubscriptionId = s.ToolSubscriptionId,
                        ToolId = s.ToolId,
                        Vendor = s.Vendor,
                        Username = s.Username,
                        Password = s.Password
                    })))
                .ForMember(m => m.Deposits, opt => opt.MapFrom(d => d.Deposits.Select(s =>
                    new AirProToolDepositEntityModel
                    {
                        ToolDepositId = s.ToolDepositId,
                        ToolId = s.ToolId,
                        Date = s.Date,
                        Description = s.Description,
                        Amount = s.Amount
                    })))
                .ForMember(m => m.Shops,
                    opt => opt.MapFrom(g => g.ShopAssignments.Select(m =>
                        new AirProToolShopEntityModel { ShopGuid = m.Key })));
        }
    }
}