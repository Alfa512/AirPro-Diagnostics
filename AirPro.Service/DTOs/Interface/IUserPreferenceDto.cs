using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IUserPreferenceDto
    {
        Guid UserGuid { get; set; }
        string ControlId { get; set; }
        string SettingsJson { get; set; }
        T GetSettings<T>();
    }
}
