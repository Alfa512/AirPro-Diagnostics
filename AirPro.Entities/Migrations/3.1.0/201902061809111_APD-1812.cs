using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1812 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Diagnostic.VehicleControllers",
                c => new
                    {
                        VehicleMakeId = c.Int(nullable: false),
                        VehicleModelName = c.String(nullable: false, maxLength: 128),
                        VehicleYear = c.String(nullable: false, maxLength: 128),
                        ControllerId = c.Int(nullable: false),
                        LastRecordedDt = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => new { t.VehicleMakeId, t.VehicleModelName, t.VehicleYear, t.ControllerId })
                .ForeignKey("Diagnostic.Controllers", t => t.ControllerId, cascadeDelete: true)
                .ForeignKey("Repair.VehicleMakes", t => t.VehicleMakeId, cascadeDelete: true)
                .Index(t => t.VehicleMakeId)
                .Index(t => t.ControllerId);
            
            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_UpdateVehicleControllers");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_RunNightlyProcess");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_RunNightlyProcess");
            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_UpdateVehicleControllers");

            DropForeignKey("Diagnostic.VehicleControllers", "VehicleMakeId", "Repair.VehicleMakes");
            DropForeignKey("Diagnostic.VehicleControllers", "ControllerId", "Diagnostic.Controllers");
            DropIndex("Diagnostic.VehicleControllers", new[] { "ControllerId" });
            DropIndex("Diagnostic.VehicleControllers", new[] { "VehicleMakeId" });
            DropTable("Diagnostic.VehicleControllers");
        }
    }
}
