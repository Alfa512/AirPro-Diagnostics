using AirPro.Common.Enumerations;

namespace AirPro.Site.Models.Notes
{
    public class NoteControlViewModel
    {
        public string Title { get; set; } = "Notes";
        public NoteType Type { get; set; }
        public string Key { get; set; }
        public string User { get; internal set; }
        public bool IsReadOnly { get; set; }
    }
}