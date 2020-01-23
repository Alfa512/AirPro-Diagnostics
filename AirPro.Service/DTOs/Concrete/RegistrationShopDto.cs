using AirPro.Service.DTOs.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirPro.Service.DTOs.Concrete
{
    internal class RegistrationShopDto : IRegistrationShopDto
    {
        public int RegistrationShopId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public string Zip { get; set; }
        public bool SendToMitchellInd { get; set; }
        public int? DiscountPercentage { get; set; }
        public string CCCShopId { get; set; }
        public bool AllowAllRepairAutoClose { get; set; }
        public bool AllowAutoRepairClose { get; set; }
        public bool AllowScanAnalysisAutoClose { get; set; }
        public bool ShopFixedPriceInd { get; set; }
        public decimal? FirstScanCost { get; set; }
        public decimal? AdditionalScanCost { get; set; }
        public int? AutomaticRepairCloseDays { get; set; }

        public bool HideFromReports { get; set; }


        public bool AllowShopBillingNotification { get { return !DisableShopBillingNotification; } set { DisableShopBillingNotification = !value; } }
        public bool AllowShopStatementNotification { get { return !DisableShopStatementNotification; } set { DisableShopStatementNotification = !value; } }

        public int? DefaultInsuranceCompanyId { get; set; }

        public int? AverageVehiclesPerMonth { get; set; }

        public int? CurrencyId { get; set; }
        public int? PricingPlanId { get; set; }
        public int? EstimatePlanId { get; set; }
        public int? BillingCycleId { get; set; }

        public string InsuranceCompaniesJson { get; set; }
        public string InsuranceCompaniesPricingPlansJson { get; set; }
        public string InsuranceCompaniesEstimatePlansJson { get; set; }
        public string VehicleMakesJson { get; set; }
        public string VehicleMakesPricingPlansJson { get; set; }

        public string VehicleMakesString { get; set; }
        public string InsuranceCompaniesString { get; set; }
        public string AllowedRequestTypesString { get; set; }
        public List<int> VehicleMakes
        {
            get { return VehicleMakesString?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList() ?? new List<int>(); }
            set { VehicleMakesString = string.Join(",", value ?? new List<int>()); }
        }
        public List<int> InsuranceCompanies
        {
            get { return InsuranceCompaniesString?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList() ?? new List<int>(); }
            set { InsuranceCompaniesString = string.Join(",", value ?? new List<int>()); }
        }
        public List<int> AllowedRequestTypes
        {
            get { return AllowedRequestTypesString?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList() ?? new List<int>(); }
            set { AllowedRequestTypesString = string.Join(",", value ?? new List<int>()); }
        }

        public bool DisableShopBillingNotification { get; set; }
        public bool DisableShopStatementNotification { get; set; }

        public List<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyPlans
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(InsuranceCompaniesPricingPlansJson))
                {
                    return JsonConvert.DeserializeObject<List<ShopInsuranceCompanyPlanDto>>(InsuranceCompaniesPricingPlansJson).ToList<IShopInsuranceCompanyPlanDto>();
                }

                return new List<IShopInsuranceCompanyPlanDto>();
            }
            set
            {
                if (value == null)
                {
                    InsuranceCompaniesPricingPlansJson = null;
                }
                else
                {
                    InsuranceCompaniesPricingPlansJson = JsonConvert.SerializeObject(value);
                }
            }
        }
        public List<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyEstimatePlans
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(InsuranceCompaniesEstimatePlansJson))
                {
                    return JsonConvert.DeserializeObject<List<ShopInsuranceCompanyPlanDto>>(InsuranceCompaniesEstimatePlansJson).ToList<IShopInsuranceCompanyPlanDto>();
                }

                return new List<IShopInsuranceCompanyPlanDto>();
            }
            set
            {
                if (value == null)
                {
                    InsuranceCompaniesEstimatePlansJson = null;
                }
                else
                {
                    InsuranceCompaniesEstimatePlansJson = JsonConvert.SerializeObject(value);
                }
            }
        }
        public List<IShopVehicleMakesPricingDto> ShopVehicleMakesPricingPlans
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(VehicleMakesPricingPlansJson))
                {
                    return JsonConvert.DeserializeObject<List<ShopVehicleMakesPricingDto>>(VehicleMakesPricingPlansJson).ToList<IShopVehicleMakesPricingDto>();
                }

                return new List<IShopVehicleMakesPricingDto>();
            }
            set
            {
                if (value == null)
                {
                    VehicleMakesPricingPlansJson = null;
                }
                else
                {
                    VehicleMakesPricingPlansJson = JsonConvert.SerializeObject(value);
                }
            }
        }

        public bool AllowSelfScanAssessment { get; set; }
    }
}
