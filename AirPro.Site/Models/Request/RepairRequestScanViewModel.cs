using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Attributes;

namespace AirPro.Site.Models.Request
{
    public class RepairRequestScanViewModel : IRepairRequestScanDto
    {
        [Required]
        public int OrderId { get; set; }
        [Required, Display(Name = "Request Type")]
        public int RequestTypeID { get; set; }
        [Required, Display(Name = "Warning Indicators")]
        public ICollection<int> WarningIndicators { get; set; } //(CheckEngine, ABS, Airbag, TPMS, Stability, Security, Other)
        [Display(Name = "Other Warning Info"), DataType(DataType.MultilineText)]
        public string OtherWarningInfo { get; set; }
        [Display(Name = "Damage Description"), DataType(DataType.MultilineText)]
        public string ProblemDescription { get; set; } //Symptoms
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Required, Display(Name = "Contact")]
        public string ContactUserGuid { get; set; }

        public string ContactOther { get; set; }

        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact First Name field is required"), DataType(DataType.MultilineText), Display(Name = "Contact First Name")]
        public string ContactOtherFirstName { get; set; }

        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact Last Name field is required"), DataType(DataType.MultilineText), Display(Name = "Contact Last Name")]
        public string ContactOtherLastName { get; set; }

        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact Phone field is required"), DataType(DataType.MultilineText), Display(Name = "Contact Phone")]
        public string ContactOtherPhone { get; set; }

        [Required, Display(Name = "Request Category")]
        public int? RequestTypeCategoryId { get; set; }
        [Display(Name = "Seat Removed")]
        public bool SeatRemovedInd { get; set; }
        [Display(Name = "Device")]
        public int ToolId { get; set; }
        public List<KeyValuePair<string, string>> PositionStatementLinks { get; set; }
    }
}