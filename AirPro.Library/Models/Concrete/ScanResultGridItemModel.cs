using System;
using System.ComponentModel.DataAnnotations;

namespace AirPro.Library.Models.Concrete
{
    public class ScanResultGridItemModel
    {
        [Display(Name = "Scan ID")]
        public int ScanID { get; set; }

        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }

        [Display(Name = "Vehicle VIN")]
        public string VehicleVIN { get; set; }

        [Display(Name = "Vehicle Make")]
        public string VehicleMake { get; set; }

        [Display(Name = "Vehicle Model")]
        public string VehicleModel { get; set; }

        [Display(Name = "Vehicle Year")]
        public string VehicleYear { get; set; }

        [Display(Name = "Scan Date")]
        public DateTime? ScanDt { get; set; }

        [Display(Name = "Upload Date")]
        public DateTime UploadDt { get; set; }

        public bool VinMatches { get; set; }
    }
}
