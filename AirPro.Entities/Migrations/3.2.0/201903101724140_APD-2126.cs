using System.Reflection;
using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2126 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            this.DropObjectIfExists(DropObjectType.Function, "Scan", "udf_GetValidationRulesByRequestId");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveRequestType");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestTypes");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestTypeDisplayList");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetValidationRuleDisplayList");

            DropForeignKey("Scan.RequestTypes", "NextRequestTypeId", "Scan.RequestTypes");
            DropIndex("Scan.RequestTypes", new[] { "NextRequestTypeId" });

            CreateTable(
                "Scan.RequestTypeValidationRules",
                c => new
                    {
                        RequestTypeId = c.Int(nullable: false),
                        ValidationRuleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestTypeId, t.ValidationRuleId })
                .ForeignKey("Scan.RequestTypes", t => t.RequestTypeId, cascadeDelete: true)
                .ForeignKey("Scan.ValidationRules", t => t.ValidationRuleId, cascadeDelete: true)
                .Index(t => t.RequestTypeId)
                .Index(t => t.ValidationRuleId);
            
            CreateTable(
                "Scan.ValidationRules",
                c => new
                    {
                        ValidationRuleId = c.Int(nullable: false, identity: true),
                        ValidationRuleText = c.String(nullable: false, maxLength: 100),
                        ValidationRuleDetails = c.String(maxLength: 1000),
                        ValidationRuleSortOrder = c.Int(nullable: false),
                        ValidationRuleActiveInd = c.Boolean(nullable: false, defaultValueSql: "1"),
                    })
                .PrimaryKey(t => t.ValidationRuleId);

            Sql(@"SET IDENTITY_INSERT Scan.ValidationRules ON;
                WITH Rules (RuleOrder, RuleText)
                AS
                (
	                SELECT 1, 'Pre Scan Performed' UNION
	                SELECT 2, 'Post Scan Performed' UNION
	                SELECT 3, 'Notes Entered' UNION
	                SELECT 4, 'Codes Exist' UNION
	                SELECT 5, 'NO Codes Exist' UNION
	                SELECT 6, 'Codes Cleared (if Exist)' UNION
	                SELECT 7, 'Work Types Selected' UNION
	                SELECT 8, 'Decisions Selected'
                )

                MERGE Scan.ValidationRules AS t
                USING Rules AS s
                ON (t.ValidationRuleId = s.RuleOrder)
                WHEN NOT MATCHED BY TARGET THEN
	                INSERT (ValidationRuleId, ValidationRuleText, ValidationRuleSortOrder)
	                VALUES (s.RuleOrder, s.RuleText, s.RuleOrder)
                OUTPUT INSERTED.*;

                SET IDENTITY_INSERT Scan.ValidationRules OFF;");

            this.DropObjectIfExists(DropObjectType.View, "Scan", "vwRequestDetails");

            Sql(@"DROP INDEX IF EXISTS IX_RequestTypeId_TypeName ON Scan.RequestTypes;");
            Sql(@"DROP INDEX IF EXISTS IDX_RequestTypeId_TypeName ON Scan.RequestTypes;");

            
            CreateTable(
                "Scan.ReportValidationRules",
                c => new
                    {
                        ReportId = c.Int(nullable: false),
                        ValidationRuleId = c.Int(nullable: false),
                        ValidationRuleResultInd = c.Boolean(nullable: false),
                        ResultAcknowledgedInd = c.Boolean(),
                        ResultAcknowledgedByUserGuid = c.Guid(),
                    })
                .PrimaryKey(t => new { t.ReportId, t.ValidationRuleId })
                .ForeignKey("Scan.Reports", t => t.ReportId, cascadeDelete: true)
                .ForeignKey("Scan.ValidationRules", t => t.ValidationRuleId, cascadeDelete: true)
                .Index(t => t.ReportId)
                .Index(t => t.ValidationRuleId);
            
            AlterColumn("Scan.RequestTypes", "TypeName", c => c.String(maxLength: 100));
            AlterColumn("Scan.RequestTypes", "Instructions", c => c.String(maxLength: 800));

            Sql(@"CREATE NONCLUSTERED INDEX IX_RequestTypeId_TypeName ON Scan.RequestTypes (RequestTypeId ASC) INCLUDE (TypeName);");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Views/Scan.vwRequestDetails.sql", true);

            DropColumn("Scan.RequestTypes", "NextRequestTypeId");
            DropColumn("Scan.RequestTypes", "ReportTemplateHtml");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            this.DropObjectIfExists(DropObjectType.Function, "Scan", "udf_GetValidationRulesByRequestId");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveRequestType");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestTypes");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestTypeDisplayList");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetValidationRuleDisplayList");

            AddColumn("Scan.RequestTypes", "ReportTemplateHtml", c => c.String());
            AddColumn("Scan.RequestTypes", "NextRequestTypeId", c => c.Int());

            DropForeignKey("Scan.ReportValidationRules", "ValidationRuleId", "Scan.ValidationRules");
            DropForeignKey("Scan.ReportValidationRules", "ReportId", "Scan.Reports");
            DropForeignKey("Scan.RequestTypeValidationRules", "ValidationRuleId", "Scan.ValidationRules");
            DropForeignKey("Scan.RequestTypeValidationRules", "RequestTypeId", "Scan.RequestTypes");

            DropIndex("Scan.ReportValidationRules", new[] { "ValidationRuleId" });
            DropIndex("Scan.ReportValidationRules", new[] { "ReportId" });
            DropIndex("Scan.RequestTypeValidationRules", new[] { "ValidationRuleId" });
            DropIndex("Scan.RequestTypeValidationRules", new[] { "RequestTypeId" });

            this.DropObjectIfExists(DropObjectType.View, "Scan", "vwRequestDetails");

            Sql(@"DROP INDEX IF EXISTS IX_RequestTypeId_TypeName ON Scan.RequestTypes;");
            Sql(@"DROP INDEX IF EXISTS IDX_RequestTypeId_TypeName ON Scan.RequestTypes;");

            AlterColumn("Scan.RequestTypes", "Instructions", c => c.String());
            AlterColumn("Scan.RequestTypes", "TypeName", c => c.String());

            Sql(@"CREATE NONCLUSTERED INDEX IX_RequestTypeId_TypeName ON Scan.RequestTypes (RequestTypeId ASC) INCLUDE (TypeName);");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Views/Scan.vwRequestDetails.sql", true);

            DropTable("Scan.ReportValidationRules");
            DropTable("Scan.ValidationRules");
            DropTable("Scan.RequestTypeValidationRules");

            CreateIndex("Scan.RequestTypes", "NextRequestTypeId");

            AddForeignKey("Scan.RequestTypes", "NextRequestTypeId", "Scan.RequestTypes", "RequestTypeId");
        }
    }
}
