using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    public class TechnicianCoverageRowItemDto
    {

        public TechnicianCoverageRowItemDto(string date)
        {
            Date = date;
            Count = new List<Tuple<int, string>>();
        }

        public int Min { get; set; } = 1;
        public int Desired { get; set; } = 5;
        public string Date { get; set; }
        public List<Tuple<int, string>> Count { get; set; }
    }
}
