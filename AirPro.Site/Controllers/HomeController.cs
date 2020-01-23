using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Site.Attributes;
using AirPro.Site.Models.Home;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRoles(ApplicationRoles.SignatureGenerator), HttpGet]
        public ActionResult SignatureGenerator()
        {
            return View();
        }

        [AuthorizeRoles(ApplicationRoles.SignatureGenerator), HttpPost]
        public ActionResult SignatureGenerator(SignatureGeneratorViewModel model)
        {
            // Check Model.
            if (!ModelState.IsValid)
                return View(model);

            // Get Key Bytes.
            var secretKeyBytes = new UTF8Encoding().GetBytes(model.ApiKey);

            // Compute Signature.
            using (var hmac = new HMACSHA1(secretKeyBytes))
            {
                var encoder = new UTF8Encoding();
                var signatureBytes = encoder.GetBytes(model.ApiUrl + model.ApiBodyText.SafeLeft(200));
                var signatureHash = hmac.ComputeHash(signatureBytes);
                model.ApiSignature = Convert.ToBase64String(signatureHash);
            }

            return View(model);
        }
    }
}
