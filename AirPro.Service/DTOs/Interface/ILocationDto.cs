using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface ILocationDto
    {
        int LocationId { get; set; }

        string Name { get; set; }

        int SortOrder { get; set; }
    }
}
