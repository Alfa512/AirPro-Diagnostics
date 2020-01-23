using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AirPro.Site.Models.Home
{
    public class SignatureGeneratorViewModel
    {
        [Display(Name = "Key"), Required]
        public string ApiKey { get; set; }
        [Display(Name = "URL"), Required]
        public string ApiUrl { get; set; }
        [Display(Name = "Body"), DataType(DataType.MultilineText), Required, AllowHtml]
        public string ApiBodyText { get; set; }
        [Display(Name = "Signature")]
        public string ApiSignature { get; set; }
    }
}