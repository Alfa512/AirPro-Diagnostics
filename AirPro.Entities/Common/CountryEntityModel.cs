using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Common
{
    [Table("Countries", Schema = "Common")]
    public class CountryEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        [Required, MaxLength(2)]
        public string AlphaCode2 { get; set; }

        [Required, MaxLength(3)]
        public string AlphaCode3 { get; set; }

        [Required]
        public int NumericCodeM49 { get; set; }

        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
