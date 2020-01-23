using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Client
{
    public class ShopInsuranceCompanyPlanViewModel : IShopInsuranceCompanyPlanDto
    {
        public int InsuranceCompanyId { get; set; }
        public int PlanId { get; set; }
        public Guid ShopGuid { get; set; }
    }
}