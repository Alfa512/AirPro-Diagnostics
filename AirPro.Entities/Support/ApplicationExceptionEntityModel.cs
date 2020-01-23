using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Support
{
    [Table("ApplicationExceptions", Schema = "Support")]
    public class ApplicationExceptionEntityModel
    {
        [Key]
        public int ExceptionId { get; set; }

        [Index]
        public int? ExceptionParentId { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionStackTrace { get; set; }

        public string ExceptionObjectInfo { get; set; }

        public DateTimeOffset ExceptionOccuredDt { get; set; } = DateTimeOffset.UtcNow;
    }
}
