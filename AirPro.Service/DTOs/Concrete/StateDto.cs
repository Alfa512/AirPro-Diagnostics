using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    internal class StateDto : IStateDto
    {
        public int StateId { get; set; }
        public string StateAbbreviation { get; set; }
        public string StateName { get; set; }
        public string CountryAlphaCode2 { get; set; }
        public string CountryAlphaCode3 { get; set; }
        public string CountryName { get; set; }
    }
}