using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface INoteDto
    {
        int? NoteId { get; set; }

        string NoteKey { get; set; }
        NoteType NoteTypeId { get; set; }

        string NoteDescription { get; set; }

        DateTime UpdatedDateTime { get; set; }

        string UpdatedByUser { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}
