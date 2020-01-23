using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRegistrationOptionsDto
    {

        List<KeyValuePair<string, string>> StateSelection { get; set; }
        List<KeyValuePair<string, string>> PricingPlanSelection { get; set; }
        List<KeyValuePair<string, string>> EstimatePlanSelection { get; set; }
        List<KeyValuePair<string, string>> BillingCycleSelection { get; set; }
        List<IVehicleMakeDto> AllVehicleMakes { get; set; }
        List<IInsuranceCompanyDto> InsuranceCompanies { get; set; }
        List<KeyValuePair<string, string>> DirectPartnersSelection { get; }
        List<KeyValuePair<string, string>> InsuranceCompaniesSelection { get; }
        List<KeyValuePair<string, string>> Programms { get; }
        List<KeyValuePair<string, string>> VehicleMakes { get; }
        List<KeyValuePair<string, string>> GroupSelection { get; set; }
        List<KeyValuePair<string, string>> RequestTypeSelection { get; set; }
        List<KeyValuePair<string, string>> CurrencySelection { get; set; }
    }
}
