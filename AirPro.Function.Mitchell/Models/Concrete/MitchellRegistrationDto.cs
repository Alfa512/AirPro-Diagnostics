using AirPro.Function.Mitchell.Models.Interface;

namespace AirPro.Function.Mitchell.Models.Concrete
{
    internal class MitchellRegistrationDto : IMitchellRegistrationDto
    {
        public string MitchellAccountId { get; set; }
        public string CallbackUrl { get; set; }
        public string UserEmail { get; set; }
    }
}