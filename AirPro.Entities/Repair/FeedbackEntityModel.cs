using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Repair
{
    [Table("Feedback", Schema = "Repair")]
    public class FeedbackEntityModel : AuditBaseEntityModel
    {
        [Key, ForeignKey(nameof(Repair))]
        public int RepairId { get; set; }
        public int ResponseTimeRate { get; set; }
        public int RequestTimeRate { get; set; }
        public int TechnicianKnowledgeRate { get; set; }
        public int ReportCompletionRate { get; set; }
        public int ConcernsAddressedRate { get; set; }
        public int TechnicianCommunicationRate { get; set; }
        [MaxLength(512)]
        public string AdditionalFeedback { get; set; }
        public virtual OrderEntityModel Repair { get; set; }
    }
}
