namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1160 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetFeedbackReport");
            Sql(@"SET IDENTITY_INSERT Notification.Templates ON;

                MERGE Notification.Templates AS t
                USING
                (
	                SELECT
		                6 [NotificationTemplateId]
		                ,'RepairFeedbackEmail' [Name]
		                ,'RepairID,VehicleVIN,VehicleMake,VehicleModel,VehicleYear,ShopRONumber,ShopName,ShopPhone,ResponseTime,RequestTime,TechnicianCommunication,TechnicianKnowledge,ReportCompletion,ConcernsAddressed,AdditionalFeedback,Average,RepairLastUpdated' [Options]
		                ,'Feedback Submission Alert' [Subject]
		                ,'Feedback submitted by {ShopName} for Repair {RepairID}: <br /><br />Response Time: {ResponseTime}<br />
Request Time: {RequestTime}<br />Technician Knowledge: {TechnicianKnowledge}<br />Report Completion: {ReportCompletion}<br />Concerns Addressed: {ConcernsAddressed}<br />Technician Communication: {TechnicianCommunication}<br /><br />Additional Feedback: {AdditionalFeedback}' [EmailBody]
                ) AS s
                ON (s.NotificationTemplateId = t.NotificationTemplateId)
                WHEN NOT MATCHED THEN
	                INSERT (NotificationTemplateId, Name, Options, Subject, EmailBody, CreatedDt, CreatedByUserGuid)
	                VALUES (NotificationTemplateId, Name, Options, Subject, EmailBody, GETUTCDATE(), Common.udf_GetEmptyGuid())
                OUTPUT INSERTED.*;

                SET IDENTITY_INSERT Notification.Templates OFF;

                DBCC CHECKIDENT('Notification.Templates', RESEED);");

            Sql(@"MERGE Reporting.ReportTemplates AS t
                USING
                (
	                SELECT
		                'Repair Feedback Report' [TemplateName]
		                ,'Download of all Repair Feedback from Shops.' [TemplateDescription]
		                ,(SELECT MAX(TemplateSortOrder) + 1 FROM Reporting.ReportTemplates) [TemplateSortOrder]
		                ,'Reporting.usp_GetFeedbackReport' [ProcedureName]
		                ,'SystemReportingView' [AccessRoles]
		                ,1 [ActiveInd]
                ) AS s
                ON (t.ProcedureName = s.ProcedureName)
                WHEN NOT MATCHED BY TARGET THEN
	                INSERT (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, AccessRoles, ActiveInd)
	                VALUES (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, AccessRoles, ActiveInd)
                OUTPUT INSERTED.*;");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetFeedbackReport");
        }
    }
}
