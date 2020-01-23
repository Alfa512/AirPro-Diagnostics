using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AirPro.Entities.Diagnostics
{
    [Table("ResultTroubleCodes", Schema = "Diagnostic")]
    public class DiagnosticResultTroubleCodeEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ResultTroubleCodeId { get; set; }

        public int ResultId { get; set; }
        [ForeignKey(nameof(ResultId))]
        public virtual DiagnosticResultEntityModel Result { get; set; }

        public int ControllerId { get; set; }
        [ForeignKey(nameof(ControllerId))]
        public virtual DiagnosticControllerEntityModel Controller { get; set; }

        public int TroubleCodeId { get; set; }
        [ForeignKey(nameof(TroubleCodeId))]
        public virtual DiagnosticTroubleCodeEntityModel TroubleCode { get; set; }

        [NotMapped]
        public ICollection<string> TroubleCodeInformationList
        {
            get => JsonConvert.DeserializeObject<ICollection<string>>(TroubleCodeInformation);
            set => TroubleCodeInformation = JsonConvert.SerializeObject(value);
        }
        public string TroubleCodeInformation { get; set; }
    }
}