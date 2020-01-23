using System;

namespace AirPro.Function.Mitchell.Models.Interface
{
    public interface IMitchellRepairDto
    {
        Guid? ShopGuid { get; set; }
        string ShopRONum { get; set; }
        string VehicleVIN { get; set; }
        bool AirBagsDeployedInd { get; set; }
        bool DrivableInd { get; set; }
        string InsuranceCoName { get; set; }
        string MitchellRecId { get; set; }
        int? Odometer { get; set; }
        string RequestBody { get; set; }
    }
}