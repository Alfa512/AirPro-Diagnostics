using System;
using AirPro.Service.DTOs.Interface;
using System.Collections.Generic;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Concrete
{
    internal class InvoiceDto : IInvoiceDto
    {
        public int RepairId { get; set; }
        public RepairStatuses RepairStatus { get; set; }
        public string RepairStatusName => RepairStatus.ToString();
        public string ShopRoNumber { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string InsuranceClaimNumber { get; set; }
        public DateTime RepairLastUpdatedDt { get; set; }
        public string VehicleVIN { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleTransmission { get; set; }
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public string ShopFax { get; set; }
        public string ShopAddress1 { get; set; }
        public string ShopAddress2 { get; set; }
        public string ShopCity { get; set; }
        public string ShopState { get; set; }
        public string ShopZip { get; set; }
        public string ShopNotes { get; set; }
        public int? InvoiceId { get; set; }
        public int InvoiceCurrencyId { get; set; }
        public string InvoiceCustomerMemo { get; set; }
        public bool InvoiceCompleteInd { get; set; }
        public string InvoicedByUserName { get; set; }
        public DateTime? InvoicedDt { get; set; }
        public IEnumerable<IInvoiceLineItemDto> LineItems { get; set; }
        public bool SendNotifications { get; set; }
    }
}