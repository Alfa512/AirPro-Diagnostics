using System;
using System.Diagnostics.CodeAnalysis;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal class UpdateResultDto : IUpdateResultDto
    {
        public UpdateResultDto(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; }
        public string Message { get; }
    }
}
