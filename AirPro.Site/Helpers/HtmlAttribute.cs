using System.Web.Mvc;
using AirPro.Common.Enumerations;

namespace AirPro.Site.Helpers
{
    public static class HtmlAttribute
    {
        public static MvcHtmlString DisableOnAjax(this HtmlHelper helper, DisableButtonType buttonType, string text)
        {
            var result = $"data-{buttonType.ToString().ToLower()}-disable data-submit-mode=\"ajax\" data-loading-text=\"<i class='fa fa-spinner fa-spin'></i> {text}\"";

            return new MvcHtmlString(result);
        }
        public static MvcHtmlString DisableOnSubmit(this HtmlHelper helper, DisableButtonType buttonType, string text)
        {
            var result = $"data-{buttonType.ToString().ToLower()}-disable data-submit-mode=\"form\" data-loading-text=\"<i class='fa fa-spinner fa-spin'></i> {text}\"";

            return new MvcHtmlString(result);
        }
    }
}