using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Support
{
    [Table("RequestLogs", Schema = "Support")]
    public class RequestLogEntityModel
    {
        [Key]
        public long RequestLogId { get; private set; }
        [ForeignKey(nameof(User))]
        public Guid? UserGuid { get; private set; }
        public virtual UserEntityModel User { get; private set; }
        [MaxLength(24), Index]
        public string SessionId { get; private set; }

        [ForeignKey(nameof(UserAgent))]
        public int UserAgentId { get; private set; }
        public RequestLogUserAgentEntityModel UserAgent { get; private set; }

        [MaxLength(45)]
        public string UserAddress { get; private set; }

        public string RawUrl { get; private set; }
        [MaxLength(100)]
        public string RouteUrl { get; private set; }
        [MaxLength(100)]
        public string RouteArea { get; private set; }
        [MaxLength(100)]
        public string RouteController { get; private set; }
        [MaxLength(100)]
        public string RouteAction { get; private set; }
        [MaxLength(100)]
        public string RouteId { get; private set; }
        [MaxLength(10)]
        public string RequestMethod { get; private set; }

        public DateTimeOffset? ActionStartTime { get; private set; }
        public DateTimeOffset? ActionEndTime { get; private set; }
        public DateTimeOffset? ResultStartTime { get; private set; }
        public DateTimeOffset? ResultEndTime { get; private set; }

        [ForeignKey(nameof(ActionException))]
        public int? ActionExceptionId { get; private set; }
        public virtual RequestLogExceptionEntityModel ActionException { get; private set; }

        [ForeignKey(nameof(ResultException))]
        public int? ResultExceptionId { get; private set; }
        public virtual RequestLogExceptionEntityModel ResultException { get; private set; }
    }
}