using AirPro.Common.Enumerations;

namespace AirPro.Common.Interfaces
{
    public interface IMailingAddress
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
    }
}