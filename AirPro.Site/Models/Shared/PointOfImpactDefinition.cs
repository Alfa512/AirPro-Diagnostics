using System.Collections.Generic;

namespace AirPro.Site.Models.Shared
{
    public class PointOfImpactDefinition
    {
        public string PropertyName { get; set; }
        public List<int> Values { get; set; }
        public bool ReadOnly { get; set; }
    }
}