using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1040 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.ReportTemplates",
                c => new
                    {
                        TemplateId = c.Int(nullable: false, identity: true),
                        TemplateName = c.String(maxLength: 200),
                        TemplateDescription = c.String(maxLength: 1000),
                        TemplateSortOrder = c.Int(nullable: false),
                        ProcedureName = c.String(maxLength: 500),
                        AccessRoles = c.String(maxLength: 500),
                        ActiveInd = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TemplateId);

            Sql(@"MERGE INTO Reporting.ReportTemplates AS t
                USING (SELECT 'Shop Activity Report', 1, 'Provides the Last Date that a Repair was Created, Scan Request was Created, Report was Created, Payment was Received, or Login has occured by Shop, and count of days since for each date.', 'Reporting.usp_GetShopActivityReport', 'SystemReportingView', 1) AS s
	                (TemplateName, TemplateSortOrder, TemplateDescription, ProcedureName, AccessRoles, ActiveInd)
                ON t.TemplateName = s.TemplateName
                WHEN NOT MATCHED THEN
	                INSERT (TemplateName, TemplateSortOrder, TemplateDescription, ProcedureName, AccessRoles, ActiveInd)
	                VALUES (TemplateName, TemplateSortOrder, TemplateDescription, ProcedureName, AccessRoles, ActiveInd)
                OUTPUT inserted.*;");
        }
        
        public override void Down()
        {
            DropTable("Reporting.ReportTemplates");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetReportTemplates");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetShopActivityReport");
            this.DropObjectIfExists(DropObjectType.Function, "Common", "udf_CommaListToTable");
        }
    }
}
