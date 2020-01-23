using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1664 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Scan.CancelReasonTypes",
                c => new
                    {
                        CancelReasonTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                        NotificationTemplate = c.Int(),
                    })
                .PrimaryKey(t => t.CancelReasonTypeId);
            
            AddColumn("Scan.Reports", "CancelReasonTypeId", c => c.Int());
            CreateIndex("Scan.Reports", "CancelReasonTypeId");
            AddForeignKey("Scan.Reports", "CancelReasonTypeId", "Scan.CancelReasonTypes", "CancelReasonTypeId");

            Sql(@"SET IDENTITY_INSERT Scan.CancelReasonTypes ON;

		        MERGE Scan.CancelReasonTypes AS t
		        USING
		        (
			        SELECT
				        1 [CancelReasonTypeId]
				        ,'Repair Incomplete' [Name]
				        ,1 [Order]
				        ,NULL [NotificationTemplate]

                    UNION

                    SELECT
				        2 [CancelReasonTypeId]
				        ,'Battery Voltage to Low' [Name]
				        ,2 [Order]
				        ,NULL [NotificationTemplate]

                    UNION

                    SELECT
				        3 [CancelReasonTypeId]
				        ,'Incorrect Submission' [Name]
				        ,3 [Order]
				        ,NULL [NotificationTemplate]

                    UNION

                    SELECT
				        4 [CancelReasonTypeId]
				        ,'Scan Tool Issues' [Name]
				        ,4 [Order]
				        ,7 [NotificationTemplate]
		        ) AS s
		        ON (t.CancelReasonTypeId = s.CancelReasonTypeId)
		        WHEN NOT MATCHED THEN
			        INSERT (CancelReasonTypeId, Name, [Order], NotificationTemplate)
			        VALUES (CancelReasonTypeId, Name, [Order], NotificationTemplate)
		        OUTPUT INSERTED.*;

		        SET IDENTITY_INSERT Scan.CancelReasonTypes OFF;

                DBCC CHECKIDENT ('Scan.CancelReasonTypes', RESEED);");

            Sql(@"SET IDENTITY_INSERT Notification.Templates ON;

                MERGE Notification.Templates AS t
                USING
                (
	                SELECT
		                7 [NotificationTemplateId]
		                ,'ScanToolIssuesEmail' [Name]
		                ,'RepairId,VehicleVIN,VehicleMake,VehicleModel,VehicleYear,ShopRONumber,ShopName,ShopPhone,RequestId,ReportId,CancellationNotes,ReportNotes,TechnicianNotes' [Options]
		                ,'Scan Tool Issues #{RequestId}' [Subject]
		                ,'Scan Tool Issues #{RequestId}' [EmailBody]
                ) AS s
                ON (s.NotificationTemplateId = t.NotificationTemplateId)
                WHEN NOT MATCHED THEN
	                INSERT (NotificationTemplateId, Name, Options, Subject, EmailBody, CreatedDt, CreatedByUserGuid)
	                VALUES (NotificationTemplateId, Name, Options, Subject, EmailBody, GETUTCDATE(), Common.udf_GetEmptyGuid())
                OUTPUT INSERTED.*;

                SET IDENTITY_INSERT Notification.Templates OFF;

                DBCC CHECKIDENT('Notification.Templates', RESEED);");

            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetScanToolIssuesNotification");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetScanToolIssuesNotification");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");

            DropForeignKey("Scan.Reports", "CancelReasonTypeId", "Scan.CancelReasonTypes");
            DropIndex("Scan.Reports", new[] { "CancelReasonTypeId" });
            DropColumn("Scan.Reports", "CancelReasonTypeId");
            DropTable("Scan.CancelReasonTypes");

            Sql("DELETE FROM Notification.Templates WHERE NotificationTemplateId = 7");
        }
    }
}
