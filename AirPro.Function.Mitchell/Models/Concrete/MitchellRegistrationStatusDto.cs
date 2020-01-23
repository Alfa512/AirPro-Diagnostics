using System;
using AirPro.Function.Mitchell.Models.Interface;

namespace AirPro.Function.Mitchell.Models.Concrete
{
    internal class MitchellRegistrationStatusDto : IMitchellRegistrationStatusDto
    {
        public string MitchellAccountId { get; set; }
        public Guid ShopGuid { get; set; }
        public string Status { get; set; }
    }
}