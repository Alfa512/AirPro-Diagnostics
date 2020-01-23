using System.Collections.Generic;
using System.Web.Mvc;

namespace AirPro.Site.Models.Repairs
{
    public class CreateRepairViewModel
    {
        public IEnumerable<SelectListItem> VehicleSelectList { get; set; }
        public IEnumerable<SelectListItem> ShopSelectListItems { get; set; }

        public VehicleViewModel Vehicle { get; set; }
        public RepairViewModel RepairOrder { get; set; }
        public bool ReadOnly { get; set; }
    }
}