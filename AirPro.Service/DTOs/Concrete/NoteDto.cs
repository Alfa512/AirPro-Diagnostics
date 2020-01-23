using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class NoteDto : INoteDto
    {
        public int? NoteId { get; set; }
        public string NoteKey { get; set; }
        public NoteType NoteTypeId { get; set; }
        public string NoteDescription { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedByUser { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}
