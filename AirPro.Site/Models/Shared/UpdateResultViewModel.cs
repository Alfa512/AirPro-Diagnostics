using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Shared
{
    public class UpdateResultViewModel : IUpdateResultDto
    {
        public UpdateResultViewModel(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; }
        public string Message { get; }
    }
}