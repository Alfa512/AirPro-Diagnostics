using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Logging;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Reporting.Models.Extract;
using AirPro.Site.Attributes;
using AirPro.Site.Hubs;
using Dapper;
using OfficeOpenXml;

namespace AirPro.Site.Areas.Reporting.Controllers
{
    [AuthorizeRoles(ApplicationRoles.SystemReportingView)]
    public class ExtractController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            LoadSelectLists();

            return View("ExtractHome");
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RunReport(GenerateExtractViewModel request)
        {
            if (request == null || !ModelState.IsValid)
            {
                LoadSelectLists();

                return View("ExtractHome", request);
            }

            // Load User Offset.
            var offset = Factory.User.UserUtcOffset;

            // Build Fields.
            var fields = string.Join(", ",
                FieldList.Intersect(request.FieldList).Select(
                    s => s.EndsWith("Dt") ? $"Common.udf_GetLocalDateTime({s},'{offset}') [{s}]" : $"[{s}]"));

            // Build Filters.
            var filterList = new List<string>();
            if (request.AccountGuid.HasValue)
                filterList.Add($"AccountGuid = '{ request.AccountGuid?.ToString() }'");
            if (request.ShopGuid.HasValue)
                filterList.Add($"ShopGuid = '{ request.ShopGuid?.ToString() }'");
            if (request.RepairStatus.HasValue)
                filterList.Add($"RepairStatusId = { request.RepairStatus }");
            if (request.RequestType.HasValue)
                filterList.Add($"RequestTypeId = { request.RequestType }");
            filterList.Add($"Common.udf_GetLocalDateTime({ DateFieldList.First(d => d == request.DateFieldFilter) }, '{ offset }') BETWEEN '{ request.StartDate.ToShortDateString() }' AND '{ request.EndDate.ToShortDateString() }'");

            var filter = filterList.Count > 0 ? $"WHERE {string.Join(" AND ", filterList)}" : string.Empty;

            // Build Query.
            var query = $"SELECT { fields } FROM Reporting.ReportData { (!string.IsNullOrEmpty(filter) ? filter : "") };";

            // Load Report Table.
            var reportTable = new DataTable();
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                using (var reader = conn.ExecuteReader(query))
                {
                    reportTable.Load(reader);
                }
            }

            // Build Excel.
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataExtract");

                worksheet.Cells["A1"].LoadFromDataTable(reportTable, true);

                int index = 0;
                List<int> dateIndexes = new List<int>();
                foreach (var cell in worksheet.Cells)
                {
                    if(index == request.FieldList.Count())
                    {
                        index = 0;
                    }

                    if(cell.Text.Contains("Dt"))
                    {
                        dateIndexes.Add(index);                        
                    }
                    else if(dateIndexes.Contains(index))
                    {
                        cell.Style.Numberformat.Format = "MM/dd/yyyy";
                    }
                    index++;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                await new ClientHubMessenger().DownloadCompleted(Factory.User.UserGuid, "/Reporting/Extract", "btnSubmit");
                
                return File(stream, "pplication/vnd.ms-excel", "AirProDataExtract.xlsx");
            }
        }

        private void LoadSelectLists()
        {
            ViewBag.AccountList = (Session["ReportAccountList"] ?? (Session["ReportAccountList"] = Factory.GetDisplayList<IAccountDto>().OrderBy(a => a.Value)
                .Select(a => new SelectListItem {Value = a.Key, Text = a.Value}).ToList())) as List<SelectListItem>;

            var shopArgs = new ServiceArgs
            {
                {"HideFromReports", false}
            };

            ViewBag.ShopList = (Session["ReportShopList"] ?? (Session["ReportShopList"] = Factory.GetDisplayList<IShopDto>(shopArgs).OrderBy(s => s.Value)
                .Select(s => new SelectListItem {Value = s.Key, Text = s.Value}).ToList())) as List<SelectListItem>;

            ViewBag.RepairStatusList = Enum.GetValues(typeof(RepairStatuses)).Cast<int>()
                .Select(s => new SelectListItem {Value = s.ToString(), Text = Enum.GetName(typeof(RepairStatuses), s)})
                .ToList();

            ViewBag.FieldList = FieldList.Select(prop => new SelectListItem {Value = prop, Text = prop, Selected = true}).ToList();

            ViewBag.DateFieldList = DateFieldList.Select(prop => new SelectListItem {Value = prop, Text = prop}).ToList();
        }

        private IEnumerable<string> FieldList => (Session["ReportFieldList"] ?? (Session["ReportFieldList"] = LoadFieldList())) as IEnumerable<string>;

        private IEnumerable<string> DateFieldList => FieldList.Where(f => f.EndsWith("Dt")).AsEnumerable();

        private IEnumerable<string> LoadFieldList()
        {
            var result = new List<string>();

            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                try
                {
                    using (var reader = conn.ExecuteReader("SELECT TOP 1 * FROM Reporting.ReportData;"))
                    {
                        while (reader.Read())
                        {
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                if (!reader.GetName(i).StartsWith("Data"))
                                    result.Add(reader.GetName(i));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }

            return result;
        }
    }
}