using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Site.Areas.Reporting.Models.Templates;
using AirPro.Site.Attributes;
using Dapper;
using Microsoft.AspNet.Identity;
using System.Data;
using System.IO;
using OfficeOpenXml;

namespace AirPro.Site.Areas.Reporting.Controllers
{
    [AuthorizeRoles(ApplicationRoles.SystemReportingView)]
    public class TemplatesController : BaseController
    {
        public ActionResult Index()
        {
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var result = conn.GetReportTemplates(User.Identity.GetUserId());

                return View("TemplateHome", result);
            }
        }

        public ActionResult Download(int id)
        {
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var result = conn.GetReportTemplates(User.Identity.GetUserId()).FirstOrDefault(r => r.TemplateId == id);

                if (result == null)
                    return new HttpNotFoundResult("Report Not Found.");

                // Lookup Template.
                var procedure = conn.QueryFirstOrDefault<string>("SELECT ProcedureName FROM Reporting.ReportTemplates WHERE TemplateId = @TemplateId AND ActiveInd = 1", new { TemplateId = id });
                if (string.IsNullOrEmpty(procedure)) throw new Exception("Report Not Found");

                // Execute Report Procedure.
                var reportTable = new DataTable();
                using (var reader = conn.ExecuteReader(procedure, new { UserGuid = User.Identity.GetUserId() }, null, null, CommandType.StoredProcedure))
                {
                    reportTable.Load(reader);
                }

                // Return Excel Data.
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("ReportDownload");

                    worksheet.Cells["A1"].LoadFromDataTable(reportTable, true);

                    var stream = new MemoryStream(package.GetAsByteArray());

                    return File(stream, "pplication/vnd.ms-excel", $"{result.TemplateName.Replace(" ", "")}_{DateTime.Now:MMddyyyy}.xlsx");
                }
            }
        }
    }

    internal static class Extention
    {
        internal static IEnumerable<ReportTemplateViewModel> GetReportTemplates(this SqlConnection conn, string userGuid)
        {
            return conn.Query<ReportTemplateViewModel>("Reporting.usp_GetReportTemplates", new { UserGuid = userGuid }, null, true, null, CommandType.StoredProcedure).OrderBy(r => r.TemplateSortOrder).ToList();
        }
    }
}