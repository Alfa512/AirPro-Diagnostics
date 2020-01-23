using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Reporting.Models.Dashboard;
using AirPro.Site.Attributes;
using Dapper;

namespace AirPro.Site.Areas.Reporting.Controllers
{
    [AuthorizeRoles(ApplicationRoles.Administrator)]
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.ShopList = Factory.GetDisplayList<IShopDto>().OrderBy(s => s.Value).Select(s => new SelectListItem { Value = s.Key, Text = s.Value }).ToList();

            return View("DashboardHome");
        }

        public async Task<JsonResult> RepairsByStatus(string shop, DateTime? date)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var query = await conn.QueryAsync("Reporting.usp_GetRepairCountByStatus @UserGuid, @ShopGuid", new
                    {
                        UserGuid = Factory.User.UserGuid,
                        ShopGuid = Guid.TryParse(shop, out var shopGuid) ? (Guid?)shopGuid : null
                    });

                var statusCounts = query?.Select(r => new { status = (RepairStatuses) r.Status, count = (int) r.StatusCount }).ToList();

                var labels = statusCounts?.Select(l => l.status.ToString()).ToList();
                var data = statusCounts?.Select(c => c.count).ToList();
                var colors = new List<string>();
                foreach (var label in labels ?? new List<string>())
                {
                    switch (label)
                    {
                        case "Active":
                            colors.Add("rgb(255, 159, 64)"); // Orange
                            break;
                        case "Canceled":
                            colors.Add("rgb(255, 99, 132)"); // Red
                            break;
                        case "Completed":
                            colors.Add("rgb(54, 162, 235)"); // Blue
                            break;
                        case "Invoiced":
                            colors.Add("rgb(75, 192, 192)"); // Green
                            break;
                        default:
                            colors.Add("rgb(0, 0, 0)");
                            break;
                    }
                }

                return Json(GetJsonChartData(labels, data, colors));
            }
        }

        public async Task<JsonResult> ScansByType(string shop, DateTime? date)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                if(date == null) date = DateTime.Today;
                
                var query = await conn.QueryAsync("Reporting.usp_GetRequestTypeCountByDate @UserGuid, @ShopGuid, @SearchDate", new
                    {
                        UserGuid = Factory.User.UserGuid,
                        ShopGuid = Guid.TryParse(shop, out var shopGuid) ? (Guid?)shopGuid : null,
                        SearchDate = date
                });

                var scanTypes = query?.Select(t => new { type = (string)t.TypeName, count = (int)t.TypeCount }).ToList();

                var labels = scanTypes?.Select(l => l.type).ToList();
                var data = scanTypes?.Select(c => c.count).ToList();
                var colors = new List<string>();
                foreach (var label in labels ?? new List<string>())
                {
                    switch (label)
                    {
                        case "Quick Scan":
                            colors.Add("rgb(54, 162, 235)"); // Blue
                            break;
                        case "Diagnostic Scan":
                            colors.Add("rgb(255, 206, 86)"); // Yellow
                            break;
                        case "Completion Scan":
                            colors.Add("rgb(255, 159, 64)"); // Orange
                            break;
                        case "Follow Up Scan":
                            colors.Add("rgb(255, 99, 132)"); // Red
                            break;
                        case "Inspection Scan":
                            colors.Add("rgb(153, 102, 255)"); // Purple
                            break;
                        case "Self Scan":
                            colors.Add("rgb(75, 192, 192)"); // Green
                            break;
                        default:
                            colors.Add("grey");
                            break;
                    }
                }

                return Json(GetJsonChartData(labels, data, colors));
            }
        }

        public ActionResult TrendChart(string shop, string timeframe = "week", string metric = "count", string dimension = "type")
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                // Load Data.
                Guid shopGuid;
                var data = conn.Query("Reporting.usp_GetChartingData @ShopGuid, @Offset, @TimeFrame;",
                    new
                    {
                        ShopGuid = Guid.TryParse(shop, out shopGuid) ? (Guid?)shopGuid : null,
                        Offset = Factory.User.UserUtcOffset,
                        TimeFrame = timeframe
                    }).ToList();

                // Create Chart Datasets.
                List<ChartDataModel> datasets = new List<ChartDataModel>();
                switch (dimension)
                {
                    case "type":
                        var types = data.GroupBy(d => new { d.RequestDt, d.RequestTypeName }).OrderBy(d => d.Key.RequestDt);
                        switch (metric)
                        {
                            case "count":
                                datasets.AddRange(types.Select(d => new ChartDataModel { label = d.Key.RequestTypeName, date = d.Key.RequestDt.ToShortDateString(), value = d.Count() }).AsEnumerable());
                                break;
                            case "cycle":
                                datasets.AddRange(types.Select(d => new ChartDataModel { label = d.Key.RequestTypeName, date = d.Key.RequestDt.ToShortDateString(), value = (decimal)d.Average(c => c.AvgCycleTimeMinutes) }).AsEnumerable());
                                break;
                            case "invoice":
                                datasets.AddRange(types.Select(d => new ChartDataModel { label = d.Key.RequestTypeName, date = d.Key.RequestDt.ToShortDateString(), value = d.Sum(c => (decimal)c.SumInvoiceAmount) }).AsEnumerable());
                                break;
                        }
                        break;
                    case "tech":
                        var techs = data.GroupBy(d => new { d.RequestDt, d.RequestTechnician }).OrderBy(d => d.Key.RequestDt);
                        switch (metric)
                        {
                            case "count":
                                datasets.AddRange(techs.Select(d => new ChartDataModel { label = d.Key.RequestTechnician, date = d.Key.RequestDt.ToShortDateString(), value = d.Count() }).AsEnumerable());
                                break;
                            case "cycle":
                                datasets.AddRange(techs.Select(d => new ChartDataModel { label = d.Key.RequestTechnician, date = d.Key.RequestDt.ToShortDateString(), value = (decimal)d.Average(c => c.AvgCycleTimeMinutes) }).AsEnumerable());
                                break;
                            case "invoice":
                                datasets.AddRange(techs.Select(d => new ChartDataModel { label = d.Key.RequestTechnician, date = d.Key.RequestDt.ToShortDateString(), value = d.Sum(c => (decimal)c.SumInvoiceAmount) }).AsEnumerable());
                                break;
                        }
                        break;
                }

                // Create Result.
                var result = new
                {
                    labels = datasets.Select(ds => ds.date).Distinct(),
                    datasets = datasets.Select(ds => ds.label).Distinct().Select(e => new
                    {
                        label = e,
                        data = datasets.Where(ds => ds.label == e).Select(ds => new
                        {
                            t = ds.date,
                            y = ds.value
                        })
                    })
                };

                // Return Data.
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private object GetJsonChartData(IEnumerable<string> labels, IEnumerable data, IEnumerable colors)
        {
            var result = new
            {
                labels = labels.Select(l => l.ToString()),
                datasets = new[]
                {
                    new
                    {
                        data = data,
                        backgroundColor = colors
                    }
                }
            };

            return result;
        }
    }
}