using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AirPro.Site.Helpers
{
    public static class Extensions
    {
        public static MvcHtmlString DisableIf(this MvcHtmlString htmlString, Func<bool> expression)
        {
            if (!expression.Invoke()) return htmlString;

            var html = htmlString.ToString();
            const string disabled = "\"disabled\"";

            html = html.Insert(html.IndexOf(">", StringComparison.Ordinal), " disabled= " + disabled);

            return new MvcHtmlString(html);
        }

        public static List<SelectListItem> GetWithoutGroups(this IEnumerable<SelectListItem> items, string filter = "")
        {
            var result = items.Where(d => string.IsNullOrEmpty(filter) || d.Value == filter)
                .Select(d => new SelectListItem
                {
                    Value = d.Value,
                    Text = d.Text,
                    Disabled = d.Disabled,
                    Selected = d.Selected
                }).GroupBy(d => d.Text).Select(d => d.First()).ToList();

            return result;
        }
    }
}