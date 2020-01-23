using AirPro.Common.Enumerations;

namespace AirPro.Site.Areas.Admin.Models.Inventory
{
    public class AirProToolStatsViewModel
    {
        public ToolType Type { get; set; }
        public int TotalCount { get; set; }
        public int AssignedCount { get; set; }
        public int UnAssignedCount { get; set; }
        public int ProducedThisMonth { get; set; }
        public int ProducedLastMonth { get; set; }
        public string TypeString
        {
            get
            {
                return Type.ToString();
            }
        }
    }
}