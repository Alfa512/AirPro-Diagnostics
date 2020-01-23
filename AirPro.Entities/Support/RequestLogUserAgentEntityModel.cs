using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Support
{
    [Table("RequestLogUserAgents", Schema = "Support")]
    public class RequestLogUserAgentEntityModel
    {
        [Key]
        public int UserAgentId { get; private set; }

        public string UserAgentString { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed), Index]
        public int UserAgentHash { get; private set; }
    }
}