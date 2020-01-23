using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2252 : DbMigration
    {
        public override void Up()
        {
            Sql(@"MERGE Reporting.ReportTemplates AS t
		        USING
		        ( 
			        SELECT
				        'Outstanding Revenue Report' [TemplateName]
				        ,'Provides a Report of Active/Completed Repairs and the Estimated Cost per Repair.' [TemplateDescription]
				        ,(SELECT MAX(TemplateSortOrder) + 1 FROM Reporting.ReportTemplates) [TemplateSortOrder]
				        ,'Reporting.usp_GetEstimatedOutstandingRevenueReport' [ProcedureName]
				        ,'SystemReportingView' [AccessRoles]
				        ,1 [ActiveInd]
		        ) AS s
		        ON (t.ProcedureName = s.ProcedureName)
		        WHEN NOT MATCHED THEN
			        INSERT (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, AccessRoles, ActiveInd)
			        VALUES (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, AccessRoles, ActiveInd)
		        OUTPUT INSERTED.*;");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimatedOutstandingRevenueReport");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM Reporting.ReportTemplates WHERE ProcedureName = 'Reporting.usp_GetEstimatedOutstandingRevenueReport';");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimatedOutstandingRevenueReport");
        }
    }
}
