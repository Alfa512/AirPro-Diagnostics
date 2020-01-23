using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Help
{
    public class ReleaseNoteInfoViewModel
    {
        public IEnumerable<IReleaseNoteDto> ReleaseNotes { get; set; }
    }
}