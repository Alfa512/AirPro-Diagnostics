using System.ComponentModel.DataAnnotations;

namespace AirPro.Site.Models.Manage
{
    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Mobile Number")]
        public string PhoneNumber { get; set; }
    }
}