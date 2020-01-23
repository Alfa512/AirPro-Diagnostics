using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AirPro.Common.Enumerations;

namespace AirPro.Site.Helpers
{
    [SuppressMessage("ReSharper", "Mvc.ActionNotResolved")]
    public static class Buttons
    {
        public static MvcHtmlString BackButton()
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return BackButton(url.Action("Index"));
        }

        public static MvcHtmlString BackButton(string url)
        {
            return new MvcHtmlString(String.Format("<a href='{0}' class='btn btn-info'><i class='glyphicon glyphicon-chevron-left'></i>&nbsp;Back</a>", url));
        }

        public static MvcHtmlString EditButton(object routeValues)
        {
            StringBuilder result = new StringBuilder();
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            result.AppendLine(String.Format("<a href='{0}' class='btn btn-warning'><i class='glyphicon glyphicon-edit'></i>&nbsp;Edit</a>", url.Action("Edit", routeValues)));

            return new MvcHtmlString(result.ToString());
        }

        public static MvcHtmlString AddButton(this HtmlHelper html, string function, string text, ApplicationRoles role)
        {
            var result = string.Empty;
            if (!HttpContext.Current.User.IsInRole(role.ToString()))
            {
                return new MvcHtmlString(result);
            }

            result = $"<a onclick=\"{function}\" class=\"btn btn-sm btn-default\">" +
                     $"<i class=\"glyphicon glyphicon-plus\"></i>&nbsp;Add {text}</a>";

            return new MvcHtmlString(result);
        }
    }
}
