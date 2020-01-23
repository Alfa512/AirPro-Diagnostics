using System.Web.Mvc;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Helpers
{
    public static class Alerts
    {
        public static MvcHtmlString UpdateResultAlert(this HtmlHelper helper, IUpdateResultDto updateResult)
        {
            var result = updateResult != null ? $"<div class='alert alert-{ (updateResult.Success ? "success" : "danger") }'>{updateResult.Message}</div>" : string.Empty;

            return new MvcHtmlString(result);
        }
    }
}