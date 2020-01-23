using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AirPro.Site.Models.Shared;

namespace AirPro.Site.Helpers
{
    public static class Selections
    {
        public static MvcHtmlString PointOfImpactSelectionFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool readOnly = false)
        {
            // Get Field Data.
            var field = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // Load Field Values.
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model as IList<int>;

            // Load URL Helper.
            var url = new UrlHelper(helper.ViewContext.RequestContext);

            // Create Result.
            var result = new StringBuilder();
            var mod = new PointOfImpactDefinition
            {
                PropertyName = field.PropertyName,
                Values = fieldValue?.ToList() ?? new List<int>(),
                ReadOnly = readOnly
            };
            result.AppendLine(helper.Partial("_Poi", mod).ToString());

            // Return Result.
            return new MvcHtmlString(result.ToString());
        }

        public static MvcHtmlString MultiCheckBoxListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> items, int columns)
        {
            // Get Field Data.
            var field = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // Load Field Values.
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model as IList<int>;

            // Start Select List.
            var result = new StringBuilder();
            result.AppendLine($"<style type='text/css'> ul.selectionList {{ columns: {columns}; -webkit-columns: {columns}; -moz-columns: {columns}; list-style: none; margin: 0; }} </style>");
            result.AppendLine($"<div id='{ field.PropertyName }' class='well well-sm' style='margin: 0; background-color: white; border-color: #cccccc;'>");

            // Load Items.
            var listItems = items?.ToList();
            if (listItems?.Any() ?? false)
            {
                // Loop Through Groups.
                foreach (var groupName in listItems.Select(i => i.Group?.Name).Distinct())
                {
                    // Check Group Name.
                    if (!string.IsNullOrEmpty(groupName))
                    {
                        // Set Group Name.
                        result.AppendLine(
                            $"<hr style='margin: 2px' /><strong>{groupName}</strong><br/><hr style='margin: 2px' />");
                    }

                    // Generate Select List.
                    result.AppendLine("<ul class='selectionList'>");
                    foreach (var selectListItem in listItems.Where(i => i.Group?.Name == groupName))
                    {
                        result.AppendLine("<li>");
                        result.AppendLine(
                            $"<input type='checkbox' id='{field.PropertyName}_{selectListItem.Value}' name='{field.PropertyName}' value='{selectListItem.Value}' {(selectListItem.Selected || (fieldValue?.Contains(Convert.ToInt32(selectListItem.Value)) ?? false) ? "checked='checked'" : "")} />");
                        result.AppendLine(
                            $"<label for='{field.PropertyName}_{selectListItem.Value}'>{selectListItem.Text}</label>");
                        result.AppendLine("</li>");
                    }
                    result.AppendLine("</ul>");
                }
            }
            else
            {
                // No Items Found.
                result.AppendLine("<i style='font-size: 15px'>No Items Found</i>");
            }

            // Close Select List.
            result.AppendLine("</div>");

            return new MvcHtmlString(result.ToString());    
        }

        public static MvcHtmlString MultiCheckBoxEnumListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, Type enumType, int columns)
        {
            var selectItems = (from object t in Enum.GetValues(enumType) select new SelectListItem {Text = t.ToString(), Value = ((int) t).ToString()}).ToList();

            return MultiCheckBoxListFor(helper, expression, selectItems, columns);
        }

        public static MvcHtmlString TimeZoneSelectionList(string userTimeZoneInfoId, string dataBind = "")
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"<select id='TimeZoneInfoId' name='TimeZoneInfoId' class='form-control' style='width: auto;' data-bind='{dataBind}'>");
            var tzs = TimeZoneInfo.GetSystemTimeZones()
                .Where(t => t.DisplayName.Contains("US") || t.DisplayName.Contains("Hawaii") ||
                            t.DisplayName.Contains("Alaska") || t.DisplayName.Contains("Canada") ||
                            t.DisplayName.Contains("Indiana") || t.DisplayName.Contains("Arizona"))
                .OrderByDescending(t => t.BaseUtcOffset);
            foreach (var tz in tzs)
            {
                string selected = userTimeZoneInfoId == tz.Id ? " selected='selected'" : string.Empty;
                result.AppendLine(String.Format("<option value='{0}'{2}>{1}</option>", tz.Id, tz.DisplayName, selected));
            }
            result.AppendLine("</select>");

            return new MvcHtmlString(result.ToString());
        }
    }
}
