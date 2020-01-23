using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReportDecisionSelectionItemDto
    {
        int DecisionId { get; set; }
        string DecisionText { get; set; }
        bool DecisionSelected { get; set; }
        ReportTextSeverity DecisionTextSeverity { get; set; }
    }
}