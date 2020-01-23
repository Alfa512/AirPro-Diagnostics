using AirPro.Service.DTOs.Interface;
using System;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ShopInsuranceCompanyPlanDto : IShopInsuranceCompanyPlanDto
    {
        public int InsuranceCompanyId { get; set; }
        public int PlanId { get; set; }
        public Guid ShopGuid { get; set; }
    }
}