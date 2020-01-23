using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service
{
    static class GridPageExtension
    {
        internal static IGridPageDto<T> GetGridPageFromCollection<T>(this ICollection<T> results, ServiceArgs args)
        {
            // Load Grid Settings.
            var currentPage = (int)(args?["CurrentPage"] ?? 0);
            var rowCount = (int)(args?["RowCount"] ?? 0);
            var sortOrder = args?["SortOrder"]?.ToString();

            // Create Results.
            var result = new GridPageDto<T>
            {
                Current = currentPage,
                Total = results.Count,
            };

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sortOrder) ? results : results?.OrderBy(sortOrder);

            // Get Page.
            var page = rowCount < 0 ? sorted : sorted.Skip((currentPage - 1) * rowCount).Take(rowCount);

            // Set Page.
            result.Rows = page.ToList();
            result.RowCount = (int)result.Rows?.Count();

            return result;
        }
    }
}
