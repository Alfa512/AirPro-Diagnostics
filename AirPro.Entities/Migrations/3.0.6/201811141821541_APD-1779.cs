using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1779 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Service.CCCEstimates", "ProcessedInd", c => c.Boolean(nullable: false));
            AddColumn("Service.CCCEstimates", "ProcessedDt", c => c.DateTimeOffset(nullable: false, precision: 7));

            Sql(@"CREATE INDEX IDX_ServiceCCCEstimates_ProcessedInd ON Service.CCCEstimates(ProcessedInd) INCLUDE (EstimateId, DocumentGuid);");

            Sql(@"UPDATE ce
	                SET ce.ProcessedInd = 1
		                ,ce.ProcessedDt = o.CreatedDt
                FROM Service.CCCEstimates ce
                INNER JOIN Repair.Orders o
	                ON ce.DocumentGuid = o.CCCDocumentGuid

                UPDATE Service.CCCEstimates
	                SET ProcessedInd = 1
		                ,ProcessedDt = GETUTCDATE()
                WHERE ProcessedInd = 0");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CreateFromCCCEstimates");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CreateFromCCCEstimates");

            Sql(@"DROP INDEX IDX_ServiceCCCEstimates_ProcessedInd ON Service.CCCEstimates;");

            DropColumn("Service.CCCEstimates", "ProcessedDt");
            DropColumn("Service.CCCEstimates", "ProcessedInd");
        }
    }
}
