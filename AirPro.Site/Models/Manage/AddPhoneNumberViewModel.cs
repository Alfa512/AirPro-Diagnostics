using System.ComponentModel.DataAnnotations;

namespace AirPro.Site.Models.Manage
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Mobile Number")]
        public string Number { get; set; }
    }
}