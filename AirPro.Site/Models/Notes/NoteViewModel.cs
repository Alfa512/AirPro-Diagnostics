using System;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Notes
{
    public class NoteViewModel : INoteDto
    {
        public int? NoteId { get; set; }
        public string NoteKey { get; set; }
        public NoteType NoteTypeId { get; set; }
        [AllowHtml]
        public string NoteDescription { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedByUser { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}