using System;

namespace AirPro.Service
{
    public interface IServiceUser
    {
        Guid UserGuid { get; set; }
        string UserName { get; set; }
        Guid[] UserRoleGuids { get; set; }
        bool UserLockedOut { get; set; }
        string TimeZoneInfoId { get; set; }
        string UserUtcOffset { get; }
    }
}