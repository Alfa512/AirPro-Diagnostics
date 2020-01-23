using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2105 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Scan.WorkTypeVehicleMakes",
                c => new
                    {
                        WorkTypeId = c.Int(nullable: false),
                        VehicleMakeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkTypeId, t.VehicleMakeId })
                .ForeignKey("Repair.VehicleMakes", t => t.VehicleMakeId, cascadeDelete: true)
                .ForeignKey("Scan.WorkTypes", t => t.WorkTypeId, cascadeDelete: true)
                .Index(t => t.WorkTypeId)
                .Index(t => t.VehicleMakeId);

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetWorkType");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetWorkTypes");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetWorkTypeSearch");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetWorkTypeDisplayList");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveWorkType");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetWorkType");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetWorkTypes");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetWorkTypeSearch");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetWorkTypeDisplayList");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveWorkType");

            DropForeignKey("Scan.WorkTypeVehicleMakes", "WorkTypeId", "Scan.WorkTypes");
            DropForeignKey("Scan.WorkTypeVehicleMakes", "VehicleMakeId", "Repair.VehicleMakes");
            DropIndex("Scan.WorkTypeVehicleMakes", new[] { "VehicleMakeId" });
            DropIndex("Scan.WorkTypeVehicleMakes", new[] { "WorkTypeId" });
            DropTable("Scan.WorkTypeVehicleMakes");
        }
    }
}
