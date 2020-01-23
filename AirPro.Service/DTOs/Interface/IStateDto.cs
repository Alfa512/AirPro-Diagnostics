namespace AirPro.Service.DTOs.Interface
{
    public interface IStateDto
    {
        int StateId { get; set; }
        string StateAbbreviation { get; set; }
        string StateName { get; set; }
        string CountryAlphaCode2 { get; set; }
        string CountryAlphaCode3 { get; set; }
        string CountryName { get; set; }
    }
}
