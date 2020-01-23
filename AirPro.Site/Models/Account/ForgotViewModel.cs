using System.ComponentModel.DataAnnotations;

namespace AirPro.Site.Models.Account
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}