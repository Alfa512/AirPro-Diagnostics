namespace AirPro.Function.Mitchell.Models.Interface
{
    public interface IMitchellRegistrationDto
    {
        string MitchellAccountId { get; set; }
        string CallbackUrl { get; set; }
        string UserEmail { get; set; }
    }
}