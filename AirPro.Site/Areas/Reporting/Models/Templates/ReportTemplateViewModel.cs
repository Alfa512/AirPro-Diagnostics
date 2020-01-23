namespace AirPro.Site.Areas.Reporting.Models.Templates
{
    public class ReportTemplateViewModel
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public int TemplateSortOrder { get; set; }
    }
}