using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    internal class RegistrationOptionsDto : IRegistrationOptionsDto
    {
        public List<KeyValuePair<string, string>> StateSelection { get; set; }
        public List<KeyValuePair<string, string>> PricingPlanSelection { get; set; }
        public List<KeyValuePair<string, string>> EstimatePlanSelection { get; set; }
        public List<KeyValuePair<string, string>> BillingCycleSelection { get; set; }
        public List<IVehicleMakeDto> AllVehicleMakes { get; set; }
        public List<IInsuranceCompanyDto> InsuranceCompanies { get; set; }

        public List<KeyValuePair<string, string>> DirectPartnersSelection
        {
            get
            {
                if (InsuranceCompanies != null)
                {
                    return InsuranceCompanies
                        .Where(m => !string.IsNullOrEmpty(m.ProgramName))
                        .Select(d => new KeyValuePair<string, string>(d.InsuranceCompanyId.ToString(), d.ProgramName))
                        .OrderBy(m => m.Value)
                        .ToList();
                }

                return new List<KeyValuePair<string, string>>();
            }
        }
        public List<KeyValuePair<string, string>> InsuranceCompaniesSelection
        {
            get
            {
                if (InsuranceCompanies != null)
                {
                    var result = InsuranceCompanies
                        .Where(m => !string.IsNullOrEmpty(m.InsuranceCompanyName))
                        .Select(d => new KeyValuePair<string, string>(d.InsuranceCompanyId.ToString(), d.InsuranceCompanyName))
                        .OrderBy(m => m.Value)
                        .ToList();
                    result.Insert(0, new KeyValuePair<string, string>("0", @"<-- Not Assigned -->"));
                    return result;
                }

                return new List<KeyValuePair<string, string>>();
            }
        }
        public List<KeyValuePair<string, string>> Programms
        {
            get
            {
                if (AllVehicleMakes != null)
                {
                    return AllVehicleMakes
                        .Where(m => !string.IsNullOrEmpty(m.ProgramName))
                        .Select(d => new KeyValuePair<string, string>(d.VehicleMakeId.ToString(), d.ProgramName))
                        .OrderBy(m => m.Value)
                        .ToList();
                }

                return new List<KeyValuePair<string, string>>();
            }
        }
        public List<KeyValuePair<string, string>> VehicleMakes
        {
            get
            {
                if (AllVehicleMakes != null)
                {
                    return AllVehicleMakes
                        .Select(d => new KeyValuePair<string, string>(d.VehicleMakeId.ToString(), d.VehicleMakeName.ToString()))
                        .OrderBy(m => m.Value)
                        .ToList();
                }

                return new List<KeyValuePair<string, string>>();
            }
        }
        public List<KeyValuePair<string, string>> GroupSelection { get; set; }
        public List<KeyValuePair<string, string>> RequestTypeSelection { get; set; }
        public List<KeyValuePair<string, string>> CurrencySelection { get; set; }
    }
}
