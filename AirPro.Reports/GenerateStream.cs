using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace AirPro.Reports
{
    internal static class GenerateStream
    {
        internal static async Task<Stream> ReportToPdfAsync(string reportPath, ReportDataSource[] dataSources)
        {
            var reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Height = Unit.Percentage(100),
                Width = Unit.Percentage(100)
            };

            var assem = Assembly.GetExecutingAssembly();
            var report = assem.GetManifestResourceStream($"{assem.GetName().Name}.{reportPath}");
            reportViewer.LocalReport.LoadReportDefinition(report);

            if (dataSources != null)
                foreach (var dataSource in dataSources)
                    reportViewer.LocalReport.DataSources.Add(dataSource);

            byte[] render = await Task.Run(() => reportViewer.LocalReport.Render("PDF"));

            Stream output = new MemoryStream();
            await output.WriteAsync(render, 0, (int)render.Length);
            output.Position = 0;

            return output;
        }
    }
}
