using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class AirProToolDepositDto : IAirProToolDepositDto
    {
        public int ToolDepositId { get; set; }
        public int ToolId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool DeleteInd { get; set; }
    }
}