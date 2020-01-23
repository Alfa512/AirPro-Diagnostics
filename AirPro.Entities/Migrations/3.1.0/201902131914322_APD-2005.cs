using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2005 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Repair.VehicleMakeTools",
                c => new
                {
                    VehicleMakeToolId = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    VehicleMakeId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.VehicleMakeToolId)
                .ForeignKey("Repair.VehicleMakes", t => t.VehicleMakeId, cascadeDelete: true)
                .Index(t => t.VehicleMakeId);

            CreateTable(
                "Scan.ReportVehicleMakeTools",
                c => new
                {
                    ReportId = c.Int(nullable: false),
                    VehicleMakeToolId = c.Int(nullable: false),
                    ToolVersion = c.String(),
                })
                .PrimaryKey(t => new { t.ReportId, t.VehicleMakeToolId })
                .ForeignKey("Scan.Reports", t => t.ReportId, cascadeDelete: true)
                .ForeignKey("Repair.VehicleMakeTools", t => t.VehicleMakeToolId, cascadeDelete: true)
                .Index(t => t.ReportId)
                .Index(t => t.VehicleMakeToolId);

            AddColumn("Repair.VehicleMakes", "ProgramInstructions", c => c.String());

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetVehicleMakes");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestById");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_VehicleMakeTool");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetVehicleMakes");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestById");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_VehicleMakeTool");

            DropForeignKey("Scan.ReportVehicleMakeTools", "VehicleMakeToolId", "Repair.VehicleMakeTools");
            DropForeignKey("Scan.ReportVehicleMakeTools", "ReportId", "Scan.Reports");
            DropForeignKey("Repair.VehicleMakeTools", "VehicleMakeId", "Repair.VehicleMakes");
            DropIndex("Scan.ReportVehicleMakeTools", new[] { "VehicleMakeToolId" });
            DropIndex("Scan.ReportVehicleMakeTools", new[] { "ReportId" });
            DropIndex("Repair.VehicleMakeTools", new[] { "VehicleMakeId" });
            DropColumn("Repair.VehicleMakes", "ProgramInstructions");
            DropTable("Scan.ReportVehicleMakeTools");
            DropTable("Repair.VehicleMakeTools");
        }
    }
}
