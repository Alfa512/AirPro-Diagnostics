using System;

namespace AirPro.Site.Areas.Admin.Models.Inventory
{
    public class AirProToolDepositViewModel
    {
        public int ToolDepositId { get; set; }
        public int ToolId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool DeleteInd { get; set; }
    }
}