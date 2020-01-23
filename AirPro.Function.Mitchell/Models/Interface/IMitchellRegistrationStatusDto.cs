using System;

namespace AirPro.Function.Mitchell.Models.Interface
{
    public interface IMitchellRegistrationStatusDto
    {
        string MitchellAccountId { get; set; }
        Guid ShopGuid { get; set; }
        string Status { get; set; }
    }
}