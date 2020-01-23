using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class CurrencyDto : ICurrencyDto
    {
        public int CurrencyId { get; set; }
        public string Name { get; set; }
    }
}
