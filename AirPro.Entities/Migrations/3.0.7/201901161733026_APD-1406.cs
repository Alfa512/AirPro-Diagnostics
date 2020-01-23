namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1406 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS (SELECT 1 FROM Technician.Profiles WHERE ActiveInd = 1)
                    UPDATE Technician.Profiles SET ActiveInd = 1 WHERE ActiveInd = 0;");

            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_GetWeeklySchedule");
            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_GetQueueConnections");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetScanReportDataSource");

            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_GetWeeklySchedule");
            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_GetQueueConnections");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetScanReportDataSource");

            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
        }
    }
}

