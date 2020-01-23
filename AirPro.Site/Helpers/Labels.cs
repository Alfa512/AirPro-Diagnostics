using System;
using System.Web.Mvc;

namespace AirPro.Site.Helpers
{
    public static class Labels
    {
        public static MvcHtmlString ConfirmedLabel(bool confirmed)
        {
            string label = String.Format("<span style='color: {0}'>({1}Confirmed)</span>", confirmed ? "green" : "red", confirmed ? string.Empty : "Not ");

            return new MvcHtmlString(label);
        }

        public static MvcHtmlString BooleanIndicator(bool value)
        {
            string result = $"<i class='glyphicon glyphicon-{(value ? "ok" : "remove")}-circle' style='color: {(value ? "green" : "red")};'></i>";

            return new MvcHtmlString(result);
        }
    }
}
