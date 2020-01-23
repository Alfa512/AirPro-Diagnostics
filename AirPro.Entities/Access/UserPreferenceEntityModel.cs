using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Access
{
    [Table("UserPreferences", Schema = "Access")]
    public class UserPreferenceEntityModel
    {
        [Key, Column(Order = 1)]
        public Guid UserGuid { get; set; }
        [Key, Column(Order = 2)]
        public string ControlId { get; set; }
        public string SettingsJson { get; set; }
        public DateTimeOffset UpdatedDt { get; set; }
    }
}
