using System;
using System.Configuration;
using System.Globalization;
using AirPro.Notifications.WebJob.Models;
using UniMatrix.Common.Extensions;

namespace AirPro.Notifications.WebJob
{
    internal static class Templates
    {
        private static readonly string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationBaseUrl"];

        internal static string ScanRequestEmailTemplate(string template, RequestNotificationModel request)
        {
            if (template == null) template = string.Empty;
            if (request == null) throw new ArgumentNullException(nameof(request));

            return template.Replace("{RequestID}", request.RequestId.ToString())
                .Replace("{RequestLink}", $"<a href='{ApplicationUrl}/Request/Report/{request.RequestId}'>View Request</a>")
                .Replace("{RequestType}", request.RequestType)
                .Replace("{ProblemDesc}", request.ProblemDescription)
                .Replace("{RepairID}", request.RepairId.ToString())
                .Replace("{VehicleVIN}", request.VehicleVIN)
                .Replace("{VehicleMake}", request.VehicleMake)
                .Replace("{VehicleModel}", request.VehicleModel)
                .Replace("{VehicleYear}", request.VehicleYear)
                .Replace("{ShopRONumber}", request.ShopRONumber)
                .Replace("{ShopName}", request.ShopName)
                .Replace("{ShopPhone}", request.ShopPhone);
        }

        internal static string ShopReportEmailTemplate(string template, RequestNotificationModel request)
        {
            if (template == null) template = string.Empty;
            if (request == null) throw new ArgumentNullException(nameof(request));

            return template.Replace("{RequestID}", request.RequestId.ToString())
                    .Replace("{ReportLink}", $"<a href='{ApplicationUrl}/Download/ScanReport/{request.RequestId}'>View Report</a>")
                    .Replace("{RequestType}", request.RequestType)
                    .Replace("{ProblemDesc}", request.ProblemDescription)
                    .Replace("{RepairID}", request.RepairId.ToString())
                    .Replace("{VehicleVIN}", request.VehicleVIN)
                    .Replace("{VehicleMake}", request.VehicleMake)
                    .Replace("{VehicleModel}", request.VehicleModel)
                    .Replace("{VehicleYear}", request.VehicleYear)
                    .Replace("{ShopRONumber}", request.ShopRONumber)
                    .Replace("{ShopName}", request.ShopName);
        }

        internal static string ShopStatementEmailTemplate(string template, StatementNotificationModel request)
        {
            if (template == null) template = string.Empty;
            if (request == null) throw new ArgumentNullException(nameof(request));

            return template
                .Replace("{PaymentID}", request.PaymentId.ToString())
                .Replace("{DiscountPercentage}", request.DiscountPercentage.ToString())
                .Replace("{PaymentCurrency}", request.PaymentCurrency)
                .Replace("{PaymentMemo}", request.PaymentMemo)
                .Replace("{PaymentReferenceNumber}", request.PaymentReferenceNumber)
                .Replace("{PaymentType}", request.PaymentType)
                .Replace("{ShopName}", request.ShopName)
                .Replace("{StatementLink}", $"<a href='{ApplicationUrl}/Download/Statement/{request.PaymentId}'>View Statement</a>")
                .Replace("{PaymentAmount}", request.PaymentAmount.ToString("C"));
        }

        internal static string ShopInvoiceEmailTemplate(string template, RepairNotificationModel repair)
        {
            if (template == null) template = string.Empty;
            if (repair == null) throw new ArgumentNullException(nameof(repair));

            return template.Replace("{InvoiceID}", repair.RepairId.ToString())
                    .Replace("{InvoiceLink}", $"<a href='{ApplicationUrl}/Download/Invoice/{repair.RepairId}'>View Invoice</a>")
                    .Replace("{InvoiceTotal}", repair.InvoiceTotal.ToString("C"))
                    .Replace("{RepairID}", repair.RepairId.ToString())
                    .Replace("{VehicleVIN}", repair.VehicleVIN)
                    .Replace("{VehicleMake}", repair.VehicleMake)
                    .Replace("{VehicleModel}", repair.VehicleModel)
                    .Replace("{VehicleYear}", repair.VehicleYear)
                    .Replace("{ShopRONumber}", repair.ShopRONumber)
                    .Replace("{ShopName}", repair.ShopName);
        }

        internal static string BillingEmailTemplate(string template, RepairNotificationModel repair)
        {
            if (template == null) template = string.Empty;
            if (repair == null) throw new ArgumentNullException(nameof(repair));

            return template.Replace("{InvoiceID}", repair.RepairId.ToString())
                    .Replace("{InvoiceLink}", $"<a href='{ApplicationUrl}/Download/Invoice/{repair.RepairId}'>View Invoice</a>")
                    .Replace("{InvoiceTotal}", repair.InvoiceTotal.ToString("C"))
                    .Replace("{RepairID}", repair.RepairId.ToString())
                    .Replace("{VehicleVIN}", repair.VehicleVIN)
                    .Replace("{VehicleMake}", repair.VehicleMake)
                    .Replace("{VehicleModel}", repair.VehicleModel)
                    .Replace("{VehicleYear}", repair.VehicleYear)
                    .Replace("{ShopRONumber}", repair.ShopRONumber)
                    .Replace("{ShopName}", repair.ShopName)
                    .Replace("{ShopPhone}", repair.ShopPhone);
        }

        internal static string RepairFeedbackTemplate(string template, RepairFeedbackNotificationModel feedback)
        {
            if (template == null) template = string.Empty;
            if (feedback == null) throw new ArgumentNullException(nameof(feedback));

            return template.Replace("{RepairID}", feedback.RepairId.ToString())
                    .Replace("{VehicleVIN}", feedback.VehicleVIN)
                    .Replace("{VehicleMake}", feedback.VehicleMake)
                    .Replace("{VehicleModel}", feedback.VehicleModel)
                    .Replace("{VehicleYear}", feedback.VehicleYear)
                    .Replace("{ShopRONumber}", feedback.ShopRONumber)
                    .Replace("{ShopName}", feedback.ShopName)
                    .Replace("{ShopPhone}", feedback.ShopPhone)

                    .Replace("{ResponseTime}", feedback.ResponseTime.ToString())
                    .Replace("{RequestTime}", feedback.RequestTime.ToString())
                    .Replace("{TechnicianCommunication}", feedback.TechnicianCommunication.ToString())
                    .Replace("{TechnicianKnowledge}", feedback.TechnicianKnowledge.ToString())
                    .Replace("{ReportCompletion}", feedback.ReportCompletion.ToString())
                    .Replace("{ConcernsAddressed}", feedback.ConcernsAddressed.ToString())
                    .Replace("{AdditionalFeedback}", feedback.AdditionalFeedback)
                    .Replace("{Average}", feedback.Average.ToString(CultureInfo.InvariantCulture))
                    .Replace("{RepairLastUpdated}", feedback.RepairLastUpdated.ToShortDateString());
        }

        internal static string RegistrationTemplate(string template, RegistrationNotificationModel registration)
        {
            if (template == null) template = string.Empty;
            if (registration == null) throw new ArgumentNullException(nameof(registration));

            return template.Replace("{RegistrationId}", registration.RegistrationId.ToString())
                    .Replace("{RegistrationUrl}", $"{ApplicationUrl}/Client/Registration/{registration.RegistrationId.ToShortGuid()}")
                    .Replace("{ClientName}", registration.ClientName)
                    .Replace("{CreatedBy}", registration.CreatedBy);
        }

        internal static string RegistrationWelcomeTemplate(string template, RegistrationWelcomeNotificationModel registration)
        {
            if (template == null) template = string.Empty;
            if (registration == null) throw new ArgumentNullException(nameof(registration));

            return template.Replace("{RegistrationId}", registration.RegistrationId.ToString())
                    .Replace("{LoginUrl}", $"{ApplicationUrl}/Account/Login")
                    .Replace("{ClientName}", registration.ClientName)
                    .Replace("{FirstName}", registration.FirstName)
                    .Replace("{LastName}", registration.LastName)
                    .Replace("{JobTitle}", registration.JobTitle)
                    .Replace("{ShopName}", registration.ShopName)
                    .Replace("{Email}", registration.Email)
                    .Replace("{AccountName}", registration.AccountName)
                    .Replace("{CreatedBy}", registration.CreatedBy);
        }

        internal static string ScanToolIssueTemplate(string template, ScanToolIssuesNotificationModel report)
        {
            if (template == null) template = string.Empty;
            if (report == null) throw new ArgumentNullException(nameof(report));

            return template.Replace("{RepairId}", report.RepairId.ToString())
                    .Replace("{VehicleVIN}", report.VehicleVIN)
                    .Replace("{VehicleMake}", report.VehicleMake)
                    .Replace("{VehicleModel}", report.VehicleModel)
                    .Replace("{VehicleYear}", report.VehicleYear)
                    .Replace("{ShopRONumber}", report.ShopRONumber)
                    .Replace("{ShopName}", report.ShopName)
                    .Replace("{ShopPhone}", report.ShopPhone)

                    .Replace("{CancellationNotes}", report.CancellationNotes)
                    .Replace("{TechnicianNotes}", report.TechnicianNotes)
                    .Replace("{ReportNotes}", report.ReportNotes)
                    .Replace("{ShopGuid}", report.ShopGuid.ToString())
                    .Replace("{RequestId}", report.RequestId.ToString())
                    .Replace("{ReportId}", report.ReportId.ToString());
        }
    }
}
