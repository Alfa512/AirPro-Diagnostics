using System.ComponentModel.DataAnnotations;

namespace AirPro.Library.Models.Concrete
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Mobile Number")]
        public string Number { get; set; }
    }
}