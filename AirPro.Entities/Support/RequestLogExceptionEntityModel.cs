using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Support
{
    [Table("RequestLogExceptions", Schema = "Support")]
    public class RequestLogExceptionEntityModel
    {
        [Key]
        public int ExcptionId { get; private set; }

        public string ExceptionMessage { get; private set; }

        public string ExceptionStackTrace { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed), Index]
        public int ExceptionHash { get; private set; }
    }
}