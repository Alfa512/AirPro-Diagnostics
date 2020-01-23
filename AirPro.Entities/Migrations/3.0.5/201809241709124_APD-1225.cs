using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1225 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_TechAvgScansByDayReport");

            Sql(@"MERGE Reporting.ReportTemplates AS t
                    USING
                    (
	                    SELECT 'Tech Average Scans Report' [TemplateName]
		                    ,'Provides the Average Scans Completed by Technician, 7 Days, 30 Days, Overall and Total Scan Count.' [TemplateDescription]
		                    ,(SELECT MAX(TemplateSortOrder) FROM Reporting.ReportTemplates) [TemplateSortOrder]
		                    ,'Reporting.usp_TechAvgScansByDayReport' [ProcedureName]
		                    ,'SystemReportingView' [AccessRoles]
		                    ,1 [ActiveInd]
                    ) AS s
                    ON (t.ProcedureName = s.ProcedureName)
                    WHEN NOT MATCHED THEN
	                    INSERT (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, AccessRoles, ActiveInd)
	                    VALUES (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, AccessRoles, ActiveInd)
                    OUTPUT INSERTED.*;");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_TechAvgScansByDayReport");

            Sql(@"DELETE FROM Reporting.ReportTemplates WHERE ProcedureName = 'Reporting.usp_TechAvgScansByDayReport';");
        }
    }
}
