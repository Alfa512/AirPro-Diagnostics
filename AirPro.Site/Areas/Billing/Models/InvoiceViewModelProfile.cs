using System.Globalization;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Billing.Models
{
    public class InvoiceViewModelProfile : Profile
    {
        public InvoiceViewModelProfile()
        {
            CreateMap<IInvoiceDto, InvoiceViewModel>().ReverseMap();
            CreateMap<IInvoiceWorkItemDto, InvoiceWorkItemViewModel>();
            CreateMap<InvoiceWorkItemViewModel, IInvoiceWorkItemDto>()
                .ForMember(d => d.InvoicedAmount,
                    opt => opt.ResolveUsing(s =>
                        decimal.TryParse(s.InvoicedAmount, NumberStyles.Any, null, out var amt) ? amt : 0));
            CreateMap<InvoiceLineItemViewModel, IInvoiceLineItemDto>()
                .ForMember(d => d.InvoicedAmount, opt => opt.ResolveUsing(s => decimal.TryParse(s.InvoicedAmount, NumberStyles.Any, null, out var amt) ? amt : 0))
                .ForMember(d => d.RequestTypeId, opt => opt.Ignore())
                .ForMember(d => d.RequestSortOrder, opt => opt.Ignore())
                .ForMember(d => d.RequestGeneratedMemo, opt => opt.Ignore());
        }
    }
}