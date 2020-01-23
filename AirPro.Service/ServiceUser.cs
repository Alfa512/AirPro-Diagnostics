using System;

namespace AirPro.Service
{
    internal class ServiceUser : IServiceUser
    {
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
        public string TimeZoneInfoId { get; set; }
        public Guid[] UserRoleGuids { get; set; }
        public bool UserLockedOut { get; set; }
        public string UserUtcOffset => TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfoId).GetUtcOffset(DateTimeOffset.UtcNow).ToString();
    }
}