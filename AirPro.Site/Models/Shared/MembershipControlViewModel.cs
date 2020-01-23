using System;
using System.Collections.Generic;

namespace AirPro.Site.Models.Shared
{
    public class MembershipControlViewModel
    {
        public string PropertyName { get; set; }
        public string MembershipTypeName { get; set; }
        public IEnumerable<KeyValuePair<string, string>> SelectionList { get; set; }
        public IEnumerable<Guid> SelectedItems { get; set; }
        public bool CanEdit { get; internal set; }
    }
}