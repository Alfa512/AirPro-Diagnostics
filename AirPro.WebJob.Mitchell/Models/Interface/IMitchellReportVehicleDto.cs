namespace AirPro.WebJob.Mitchell.Models.Interface
{
    public interface IMitchellReportVehicleDto
    {
        string Year { get; set; }
        string Make { get; set; }
        string Model { get; set; }
        string Body { get; set; }
        string Engine { get; set; }
        string Vin { get; set; }
        string VinVerified { get; set; }
    }
}