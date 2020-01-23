using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AirPro.Entities.Diagnostics
{
    [Table("ResultFreezeFrames", Schema = "Diagnostic")]
    public class DiagnosticResultFreezeFrameEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FreezeFrameId { get; set; }

        public int ResultId { get; set; }
        [ForeignKey(nameof(ResultId))]
        public virtual DiagnosticResultEntityModel Result { get; set; }

        public int ControllerId { get; set; }
        [ForeignKey(nameof(ControllerId))]
        public virtual DiagnosticControllerEntityModel Controller { get; set; }

        [MaxLength(100)]
        public string FreezeFrameTroubleCode { get; set; }

        [NotMapped]
        public ICollection<DiagnosticResultFreezeFrameSensorGroupEntityModel> SensorGroups
        {
            get => JsonConvert.DeserializeObject<ICollection<DiagnosticResultFreezeFrameSensorGroupEntityModel>>(SensorGroupsJson ?? "[]");
            set => SensorGroupsJson = JsonConvert.SerializeObject(value);
        }

        public string SensorGroupsJson { get; private set; }
    }
}
