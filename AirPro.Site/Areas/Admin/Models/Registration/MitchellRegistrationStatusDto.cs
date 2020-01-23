using System;
using AirPro.Function.Mitchell.Models.Interface;

namespace AirPro.Site.Areas.Admin.Models.Registration
{
    internal class MitchellRegistrationStatusDto : IMitchellRegistrationStatusDto
    {
        public string MitchellAccountId { get; set; }
        public Guid ShopGuid { get; set; }
        public string Status { get; set; }
    }
}