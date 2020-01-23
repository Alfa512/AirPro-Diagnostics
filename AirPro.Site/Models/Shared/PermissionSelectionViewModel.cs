using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPro.Site.Models.Shared
{
    public class PermissionSelectionViewModel
    {
        public string FieldName { get; set; }
        public ICollection<Guid> PermissionGuids { get; set; }
    }
}