using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace AirPro.Library.Models.Concrete
{
    public class IndexViewModel
    {
        public string Email { get; set; }
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public string TimeZoneInfoId { get; set; }
    }
}