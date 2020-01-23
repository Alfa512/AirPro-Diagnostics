﻿using System;
using AirPro.WebJob.Mitchell.Models.Interface;

namespace AirPro.WebJob.Mitchell.Models.Concrete
{
    [Serializable]
    internal class MitchellReportVehicleDto : IMitchellReportVehicleDto
    {
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Body { get; set; }
        public string Engine { get; set; }
        public string Vin { get; set; }
        public string VinVerified { get; set; }
    }
}