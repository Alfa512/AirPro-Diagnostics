using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service
{
    public static class DtoExtensions
    {
        public static string GetReportDtoHtml(this IReportDto report)
        {
            // Create Result.
            var result = new StringBuilder();

            // Add Report Header.
            result.AppendLine(report.ReportHeaderHTML);

            var selectedTools = report.VehicleMakeTools.Where(x => x.CheckedInd && x.ToolVersion != "NA" && x.ToolVersion != "na").ToList();
            if (selectedTools.Any())
            {
                result.AppendLine(HtmlNewLine);
                result.AppendLine(HtmlHeader("OEM Tools Used:"));
                foreach (var tool in selectedTools)
                {
                    result.AppendLine(HtmlIndent + $"{tool.Name} - Version # {tool.ToolVersion}" + HtmlNewLine);
                }
            }

            // Add Work Types.
            if (report.WorkTypeSelections.Any(t => t.WorkTypeSelected))
            {
                result.AppendLine(HtmlNewLine);
                result.AppendLine(HtmlHeader("Work Performed:"));
                result.AppendLine("<ul>");
                foreach (var reportWorkTypeSelectionItemDto in report.WorkTypeSelections.Where(t => t.WorkTypeSelected))
                {
                    result.AppendLine(HtmlListItem(reportWorkTypeSelectionItemDto.WorkTypeName));
                }
                result.AppendLine("</ul>");
            }

            // Add Decisions.
            if (report.DecisionSelections.Any(d => d.DecisionSelected))
            {
                result.AppendLine(HtmlNewLine);
                result.AppendLine(HtmlHeader("Recommendations:"));
                result.AppendLine("<ul>");
                foreach (var reportDecisionSelectionItemDto in report.DecisionSelections.Where(d => d.DecisionSelected))
                {
                    result.AppendLine(HtmlListItem(TextSeverityHighlight(reportDecisionSelectionItemDto.DecisionTextSeverity, reportDecisionSelectionItemDto.DecisionText)));
                }
                result.AppendLine("</ul>");
            }

            // Load Report Trouble Codes.
            var reportItemsList = report.TroubleCodeRecommendations.Where(r => r.Recommendations.Any(rc => rc.CurrentRequestInd && !rc.ExcludeFromReportInd)).Select(c => new
            {
                c.ControllerId,
                c.ControllerName,
                c.TroubleCodeId,
                c.TroubleCode,
                c.TroubleCodeDescription,
                c.Recommendations.First(a => a.CurrentRequestInd).InformCustomerInd,
                c.Recommendations.First(a => a.CurrentRequestInd).AccidentRelatedInd,
                c.Recommendations.First(a => a.CurrentRequestInd).CodeClearedInd,
                c.Recommendations.First(a => a.CurrentRequestInd).TroubleCodeRecommendationText,
                c.Recommendations.First(a => a.CurrentRequestInd).RecommendationTextSeverity,
                c.Recommendations.First(a => a.CurrentRequestInd).TroubleCodeNoteText
            }).ToList();

            // Report Summary.
            if (reportItemsList.Count > 0)
            {
                // Load DTC.
                var reportDtcList = reportItemsList.Where(i => !string.IsNullOrEmpty(i.TroubleCode)).ToList();

                // Load Controller Summary.
                var controllerWithDtcCount = reportDtcList.Where(i => !i.CodeClearedInd).Select(i => i.ControllerName).Distinct().Count();
                var controllerWithClearCount = reportDtcList.Where(i => i.CodeClearedInd).Select(i => i.ControllerName).Distinct().Count();
                var controllerTotalCount = reportItemsList.Select(i => i.ControllerName).Distinct().Count();
                var controllerWithOutDtcCount = controllerTotalCount - (controllerWithDtcCount + controllerWithClearCount);
                if (controllerTotalCount > 0 || controllerWithDtcCount > 0 || controllerWithOutDtcCount > 0)
                {
                    result.AppendLine(HtmlNewLine);
                    result.AppendLine(HtmlHeader("Controller Summary:"));
                    result.AppendLine(HtmlIndent + $"Total Scanned:  { controllerTotalCount }" + HtmlNewLine);
                    result.AppendLine(HtmlIndent + $"Cleared:  { controllerWithClearCount }" + HtmlNewLine);
                    result.AppendLine(HtmlIndent + $"Scanned w/DTCs:  { controllerWithDtcCount }" + HtmlNewLine);
                    result.AppendLine(HtmlIndent + $"Scanned w/o DTCs:  { controllerWithOutDtcCount }" + HtmlNewLine);
                }

                // Load Accident Related Summary.
                var accidentRelatedYesCount = reportDtcList.Count(i => i.AccidentRelatedInd ?? false);
                var accidentRelatedNoCount = reportDtcList.Count(i => !i.AccidentRelatedInd ?? false);
                var accidentRelatedUnknownCount = reportDtcList.Count(i => i.AccidentRelatedInd == null);
                if (accidentRelatedYesCount > 0 || accidentRelatedNoCount > 0 || accidentRelatedUnknownCount > 0)
                {
                    result.AppendLine(HtmlNewLine);
                    result.AppendLine(HtmlHeader("Accident Related DTCs:"));
                    result.AppendLine(HtmlIndent + $"Yes:  { accidentRelatedYesCount }" + HtmlNewLine);
                    result.AppendLine(HtmlIndent + $"No:  { accidentRelatedNoCount }" + HtmlNewLine);
                    result.AppendLine(HtmlIndent + $"Possible:  { accidentRelatedUnknownCount }" + HtmlNewLine);
                }

                // Display Controllers.
                var controllersWithClearDtc = reportDtcList.Where(i => i.CodeClearedInd).ToList<dynamic>();
                if (controllersWithClearDtc.Count > 0)
                    ControllersToHtml(controllersWithClearDtc, "Cleared DTCs:", ref result);

                var controllersWithDtc = reportDtcList.Where(i => !i.CodeClearedInd).ToList<dynamic>();
                if (controllersWithDtc.Count > 0)
                    ControllersToHtml(controllersWithDtc, "Controllers w/ DTCs:", ref result);

                var controllersWithOutDtc = reportItemsList.Except(reportDtcList).ToList<dynamic>();
                if (controllersWithOutDtc.Count > 0)
                    ControllersToHtml(controllersWithOutDtc, "Controllers w/o DTCs:", ref result);

                result.AppendLine(HtmlNewLine);
            }

            // Add Report Footer.
            result.AppendLine(report.ReportFooterHTML);

            // Return.
            return result.ToString().Replace($"{HtmlNewLine}\r\n{HtmlNewLine}\r\n{HtmlNewLine}\r\n", $"{HtmlNewLine}\r\n{HtmlNewLine}\r\n");
        }

        private static void ControllersToHtml(IList<dynamic> itemList, string title, ref StringBuilder result)
        {
            result.AppendLine(HtmlNewLine);
            result.AppendLine(HtmlHeader($"{title}"));
            foreach (var controllerName in itemList.Select(c => c.ControllerName).Distinct())
            {
                result.AppendLine(HtmlBold(controllerName) + HtmlNewLine);
                foreach (var troubleCode in itemList.Where(i => i.ControllerName == controllerName))
                {
                    if (troubleCode.InformCustomerInd)
                        result.AppendLine(HtmlIndent + HtmlBold("** Inform Customer **") + HtmlNewLine);
                    if (troubleCode.AccidentRelatedInd ?? false)
                        result.AppendLine(HtmlIndent + HtmlBold("** Accident Related **") + HtmlNewLine);

                    var separator = !string.IsNullOrEmpty(troubleCode.TroubleCode) && !string.IsNullOrEmpty(troubleCode.TroubleCodeDescription) ? " - " : "";
                    result.AppendLine(HtmlIndent + troubleCode.TroubleCode + separator + troubleCode.TroubleCodeDescription + HtmlNewLine);

                    if (!string.IsNullOrEmpty(troubleCode.TroubleCodeRecommendationText))
                        result.AppendLine(HtmlIndent + HtmlBold("Recommendation: ") + TextSeverityHighlight(troubleCode.RecommendationTextSeverity, troubleCode.TroubleCodeRecommendationText) + HtmlNewLine);
                    if (!string.IsNullOrEmpty(troubleCode.TroubleCodeNoteText))
                        result.AppendLine(HtmlIndent + HtmlBold("Note: ") + troubleCode.TroubleCodeNoteText + HtmlNewLine);
                }
                result.AppendLine(HtmlNewLine);
            }
        }

        private static string TextSeverityHighlight(ReportTextSeverity severity, string text)
        {
            switch (severity)
            {
                case ReportTextSeverity.Success:
                    return $"<span style='color: green;'>{text}</span>";
                case ReportTextSeverity.Warning:
                    return $"<span style='color: darkorange;'>{text}</span>";
                case ReportTextSeverity.Danger:
                    return $"<span style='color: red;'>{text}</span>";
                default:
                    return text;
            }
        }

        private const string HtmlNewLine = "<br/>";
        private const string HtmlIndent = "&nbsp;&nbsp;&nbsp;&nbsp;";
        private static string HtmlListItem(string text) => $"<li>{text}</li>";
        private static string HtmlBold(string text) => $"<strong>{text}</strong>";
        private static string HtmlHeader(string text) => HtmlBold($"<span style='font-size: 16px;'>{text}</span>") + HtmlNewLine;
    }
}