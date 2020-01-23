using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface IAirProToolDepositDto
    {
        int ToolDepositId { get; set; }
        int ToolId { get; set; }
        DateTimeOffset Date { get; set; }
        string Description { get; set; }
        decimal Amount { get; set; }
        bool DeleteInd { get; set; }
    }
}