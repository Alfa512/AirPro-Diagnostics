using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Reporting
{
    [Table("ReportTemplates", Schema = "Reporting")]
    public class ReportTemplateEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TemplateId { get; set; }
        [MaxLength(200)]
        public string TemplateName { get; set; }
        [MaxLength(1000)]
        public string TemplateDescription { get; set; }
        public int TemplateSortOrder { get; set; }
        [MaxLength(500)]
        public string ProcedureName { get; set; }
        [MaxLength(500)]
        public string AccessRoles { get; set; }
        public bool ActiveInd { get; set; } = true;
    }
}
