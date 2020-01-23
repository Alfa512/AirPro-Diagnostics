using System;
using System.Collections.Generic;
using System.Linq;
using AirPro.Reports.DataSources.Interface;

namespace AirPro.Reports.DataSources.Concrete
{
    public class RepairInvoiceLineItemsDataSource : IRepairInvoiceLineItemsDataSource
    {
        public string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
        public string ComputedWorkPerfomed
        {
            get
            {
                if (SubItems?.Count > 0)
                {
                    return String.Join("\n", SubItems.Select(x => Truncate(x.ComputedWorkPerfomed, 90)));
                }

                if (ReportId == null)
                {
                    return $"  > {WorkPerfomed}";
                }

                var parts = RequestCategoryName.Split('-');
                if (parts.Length > 0)
                {
                    var prefix = parts[0];
                    return $"{prefix} {WorkPerfomed}";
                }
                return $"{WorkPerfomed}";
            }
        }
        public string WorkPerfomed { get; set; }
        public decimal InvoiceAmount { get; set; }

        public string ComputedInvoiceAmount
        {
            get
            {
                if (SubItems?.Count > 0)
                {
                    return String.Join("\n", SubItems.Select(x => x.ComputedInvoiceAmount));
                }

                return $"${InvoiceAmount}".PadLeft(11).PadRight(0);
            }
        }
        public string RequestedBy { get; set; }
        public int? ReportId { get; set; }
        public string RequestCategoryName { get; set; }

        public List<RepairInvoiceLineItemsDataSource> SubItems { get; set; }
        public bool HasSubItems { get; set; }

        public decimal TotalItemAmount
        {
            get
            {
                return InvoiceAmount + (SubItems?.Sum(x => x.InvoiceAmount) ?? 0);
            }
        }

        public bool LastItem { get; set; }
    }
}
