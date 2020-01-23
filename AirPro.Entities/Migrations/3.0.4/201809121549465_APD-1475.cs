using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1475 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceSummaryReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetVolumeSummaryReport");

            Sql(@"MERGE Reporting.ReportTemplates AS t
                USING
                (
	                SELECT
		                'Volume Summary Report' [TemplateName]
		                ,'Provides System Volume Summary by Month broken down by Request Type.' [TemplateDescription]
		                ,(SELECT MAX(TemplateSortOrder) + 1 FROM Reporting.ReportTemplates) [TemplateSortOrder]
		                ,'Reporting.usp_GetVolumeSummaryReport' [ProcedureName]

	                UNION

	                SELECT
		                'Invoice Summary Report' [TemplateName]
		                ,'Provides Invoice Summary by Month broken down by Request Type.' [TemplateDescription]
		                ,(SELECT MAX(TemplateSortOrder) + 2 FROM Reporting.ReportTemplates) [TemplateSortOrder]
		                ,'Reporting.usp_GetInvoiceSummaryReport' [ProcedureName]
                ) AS s
                ON (s.TemplateName = t.TemplateName)
                WHEN NOT MATCHED THEN
	                INSERT (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, AccessRoles, ActiveInd)
	                VALUES (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, 'SystemReportingView', 1)
                OUTPUT INSERTED.*;");
        }

        public override void Down()
        {
            Sql(@"DELETE FROM Reporting.ReportTemplates WHERE TemplateName IN ('Volume Summary Report', 'Invoice Summary Report');");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceSummaryReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetVolumeSummaryReport");
        }
    }
}
