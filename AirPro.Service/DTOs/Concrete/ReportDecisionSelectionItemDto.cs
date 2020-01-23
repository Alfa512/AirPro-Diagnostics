using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportDecisionSelectionItemDto : IReportDecisionSelectionItemDto
    {
        public int DecisionId { get; set; }
        public string DecisionText { get; set; }
        public bool DecisionSelected { get; set; }
        public ReportTextSeverity DecisionTextSeverity { get; set; }
    }
}