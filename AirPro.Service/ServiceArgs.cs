using System.Collections.Generic;

namespace AirPro.Service
{
    public class ServiceArgs : Dictionary<string, object>
    {
        public void AddGridOptions(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            Add("SearchPhrase", searchPhrase);
            Add("CurrentPage", pageNumber);
            Add("RowCount", pageSize);
            Add("SortOrder", sort);
        }

        public void SetDefaultSort(string sortOrder)
        {
            if (ContainsKey("SortOrder"))
            {
                if (string.IsNullOrEmpty(this["SortOrder"]?.ToString()))
                    this["SortOrder"] = sortOrder;
            }
            else
            {
                Add("SortOrder", sortOrder);
            }
        }
    }
}
