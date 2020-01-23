using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface IShopInsuranceCompanyPlanDto
    {
        int InsuranceCompanyId { get; set; }
        int PlanId { get; set; }
        Guid ShopGuid { get; set; }
    }
}