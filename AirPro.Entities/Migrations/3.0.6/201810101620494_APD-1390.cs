using UniMatrix.Common.Enumerations;

namespace AirPro.Entities.Migrations
{
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Extensions;

    public partial class APD1390 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetTechAvgScansByHourReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetHourlyVolumeByWeekReport");

            Sql(@"MERGE Reporting.ReportTemplates AS t
                USING
                (
	                SELECT
		                'Hourly Volume By Week Report' [TemplateName]
		                ,'Report for the average volume per hour over the past 2 weeks including estimated volume of current week.' [TemplateDescription]
		                ,(SELECT MAX(TemplateSortOrder) + 1 FROM Reporting.ReportTemplates) [TemplateSortOrder]
		                ,'Reporting.usp_GetHourlyVolumeByWeekReport' [ProcedureName]
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
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetTechAvgScansByHourReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetHourlyVolumeByWeekReport");

            Sql(@"DELETE FROM Reporting.ReportTemplates WHERE ProcedureName = 'Reporting.usp_GetHourlyVolumeByWeekReport';");
        }
    }
}
