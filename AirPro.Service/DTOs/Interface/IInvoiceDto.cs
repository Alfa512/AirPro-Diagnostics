using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IInvoiceDto
    {
        int RepairId { get; set; }
        RepairStatuses RepairStatus { get; set; }
        string RepairStatusName { get; }
        string ShopRoNumber { get; set; }
        string InsuranceCompanyName { get; set; }
        string InsuranceClaimNumber { get; set; }
        DateTime RepairLastUpdatedDt { get; set; }

        string VehicleVIN { get; set; }
        string VehicleMake { get; set; }
        string VehicleModel { get; set; }
        string VehicleYear { get; set; }
        string VehicleTransmission { get; set; }

        string ShopName { get; set; }
        string ShopPhone { get; set; }
        string ShopFax { get; set; }
        string ShopAddress1 { get; set; }
        string ShopAddress2 { get; set; }
        string ShopCity { get; set; }
        string ShopState { get; set; }
        string ShopZip { get; set; }
        string ShopNotes { get; set; }

        int? InvoiceId { get; set; }
        int InvoiceCurrencyId { get; set; }
        string InvoiceCustomerMemo { get; set; }
        bool InvoiceCompleteInd { get; set; }
        string InvoicedByUserName { get; set; }
        DateTime? InvoicedDt { get; set; }

        IEnumerable<IInvoiceLineItemDto> LineItems { get; set; }

        bool SendNotifications { get; set; }
    }
}