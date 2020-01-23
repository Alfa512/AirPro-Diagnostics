using System.Collections.Generic;

namespace AirPro.Site.Models.Client
{
    public class RegistrationViewModel
    {
        public List<string> TimeZones = new List<string>();
        public List<string> States = new List<string>();
        public List<string> BillingCycles = new List<string>();
        public List<string> DirectRepairPartners = new List<string>();
        public List<string> OEMCertifications = new List<string>();
    }
}