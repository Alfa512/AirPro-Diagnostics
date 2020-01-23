using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1529 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_GetWeeklySchedule");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_TechAvgScansByDayReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetTechAvgScansByDayReport");

            Sql(@"UPDATE Reporting.ReportTemplates
                SET ProcedureName = 'Reporting.usp_GetTechAvgScansByDayReport'
                WHERE ProcedureName = 'Reporting.usp_TechAvgScansByDayReport'");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_GetWeeklySchedule");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_TechAvgScansByDayReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetTechAvgScansByDayReport");

            Sql(@"UPDATE Reporting.ReportTemplates
                SET ProcedureName = 'Reporting.usp_TechAvgScansByDayReport'
                WHERE ProcedureName = 'Reporting.usp_GetTechAvgScansByDayReport'");
        }
    }
}
