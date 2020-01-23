using System;
using AirPro.Function.Mitchell.Models.Interface;

namespace AirPro.Function.Mitchell.Models.Concrete
{
    [Serializable]
    internal class MitchellRepairDto : IMitchellRepairDto
    {
        public Guid? ShopGuid { get; set; }
        public string VehicleVIN { get; set; }
        public string MitchellRecId { get; set; }
        public string ShopRONum { get; set; }
        public string InsuranceCoName { get; set; }
        public int? Odometer { get; set; }
        public bool DrivableInd { get; set; }
        public bool AirBagsDeployedInd { get; set; }
        public string RequestBody { get; set; }
    }
}