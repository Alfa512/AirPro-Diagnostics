using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class UserPreferenceDto : IUserPreferenceDto
    {
        public Guid UserGuid { get; set; }
        public string ControlId { get; set; }
        public string SettingsJson { get; set; }
        public T GetSettings<T>()
        {
            if (string.IsNullOrWhiteSpace(SettingsJson)) return default(T);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(SettingsJson);
        }
    }
}
