namespace AirPro.WebJob.Mitchell.Models.Interface
{
    public interface IMitchellReportDtcDto
    {
        string Code { get; set; }
        string Description { get; set; }
        string Category { get; set; }
        string CategoryType { get; set; }
    }
}