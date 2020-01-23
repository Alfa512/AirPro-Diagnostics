using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirPro.Entities.Scan
{
    public sealed class RequestEntityMetadata
    {
        [Display(Name = "Type Of Scan")]
        public RequestTypeEntityModel RequestTypeId { get; set; }
        [Display(Name = "Warning Indicators")]
        public ICollection<RequestWarningIndicatorEntityModel> WarningIndicators { get; set; }
        [Display(Name = "Other Warning Info"), DataType(DataType.MultilineText)]
        public string OtherWarningInfo { get; set; }
        [Display(Name = "Damage Description"), DataType(DataType.MultilineText)]
        public string ProblemDescription { get; set; } //Symptoms
        [DataType(DataType.MultilineText), Display(Name = "Request Notes")]
        public string Notes { get; set; }
        [Display(Name = "Contact")]
        public Guid? ContactUserGuid { get; set; }
        [DataType(DataType.MultilineText), Display(Name = "Contact Name/Number")]
        public string Contact { get; set; }
        [Display(Name = "Seat Removed")]
        public bool SeatRemovedInd { get; set; }
    }
}
