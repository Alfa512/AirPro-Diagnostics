using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPro.Site.Models.Help
{
    public class HelpIndexViewModel
    {
        public string Version { get; set; }
        public List<string> VersionSelectListItems { get; set; }
    }
}