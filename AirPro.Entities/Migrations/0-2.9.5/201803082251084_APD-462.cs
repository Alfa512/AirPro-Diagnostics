namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD462 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Technician.Profiles",
                c => new
                    {
                        UserGuid = c.Guid(nullable: false),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        EmployeeId = c.String(nullable: false, maxLength: 128),
                        OtherNotes = c.String(),
                        ActiveInd = c.Boolean(nullable: false, defaultValueSql: "1"),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.UserGuid)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UserGuid)
                .Index(t => t.UserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Technician.ProfileVehicleMakes",
                c => new
                    {
                        UserGuid = c.Guid(nullable: false),
                        VehicleMakeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGuid, t.VehicleMakeId })
                .ForeignKey("Access.Users", t => t.UserGuid)
                .ForeignKey("Repair.VehicleMakes", t => t.VehicleMakeId)
                .ForeignKey("Technician.Profiles", t => t.UserGuid)
                .Index(t => t.UserGuid)
                .Index(t => t.VehicleMakeId);
        }
        
        public override void Down()
        {
            DropForeignKey("Technician.ProfileVehicleMakes", "UserGuid", "Technician.Profiles");
            DropForeignKey("Technician.ProfileVehicleMakes", "VehicleMakeId", "Repair.VehicleMakes");
            DropForeignKey("Technician.ProfileVehicleMakes", "UserGuid", "Access.Users");
            DropForeignKey("Technician.Profiles", "UserGuid", "Access.Users");
            DropForeignKey("Technician.Profiles", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Technician.Profiles", "CreatedByUserGuid", "Access.Users");
            DropIndex("Technician.ProfileVehicleMakes", new[] { "VehicleMakeId" });
            DropIndex("Technician.ProfileVehicleMakes", new[] { "UserGuid" });
            DropIndex("Technician.Profiles", new[] { "UpdatedByUserGuid" });
            DropIndex("Technician.Profiles", new[] { "CreatedByUserGuid" });
            DropIndex("Technician.Profiles", new[] { "UserGuid" });
            DropTable("Technician.ProfileVehicleMakes");
            DropTable("Technician.Profiles");
        }
    }
}
