namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1673 : DbMigration
    {
        public override void Up()
        {
            Sql(@"MERGE Reporting.ReportTemplates AS t
                USING
                (
	                SELECT
		                'Technician Weekly Schedule' [TemplateName]
		                ,'Report for the technician weekly schedule.' [TemplateDescription]
		                ,(SELECT MAX(TemplateSortOrder) + 1 FROM Reporting.ReportTemplates) [TemplateSortOrder]
		                ,'Technician.usp_GetWeeklySchedule' [ProcedureName]
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
            Sql(@"DELETE FROM Reporting.ReportTemplates WHERE ProcedureName = 'Technician.usp_GetWeeklySchedule';");
        }
    }
}
