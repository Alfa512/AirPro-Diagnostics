namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Scan.Requests", "CreatedBy_Id", "Access.Users");
            DropIndex("Scan.Requests", new[] { "CreatedBy_Id" });
            CreateTable(
                "Repair.VehicleLookups",
                c => new
                    {
                        LookupID = c.Int(nullable: false, identity: true),
                        VehicleVIN = c.String(nullable: false),
                        Service = c.Int(nullable: false),
                        RequestBaseURL = c.String(),
                        RequestString = c.String(),
                        RequestMessage = c.String(),
                        RequestSuccess = c.Boolean(nullable: false),
                        ResponseStatusCode = c.Int(nullable: false),
                        ResponseContent = c.String(),
                        RequestDt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.LookupID);
            
            AddColumn("Repair.Orders", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Repair.Orders", "UpdatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Repair.Orders", "Shop_ID", c => c.Int());
            AddColumn("Repair.Orders", "CreatedBy_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("Repair.Orders", "UpdatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("Repair.Photos", "FileName", c => c.String());
            AddColumn("Repair.Photos", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Repair.Photos", "UpdatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Repair.Photos", "CreatedBy_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("Repair.Photos", "UpdatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("Repair.Vehicles", "Transmission", c => c.String(nullable: false));
            AddColumn("Repair.Vehicles", "VehicleLookup_LookupID", c => c.Int());
            AddColumn("Scan.Requests", "UpdatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Scan.Requests", "UpdatedBy_Id", c => c.String(maxLength: 128));
            AlterColumn("Repair.Vehicles", "Make", c => c.String(nullable: false));
            AlterColumn("Repair.Vehicles", "Model", c => c.String(nullable: false));
            AlterColumn("Repair.Vehicles", "Year", c => c.String(nullable: false));
            AlterColumn("Scan.Requests", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("Scan.Requests", "CreatedBy_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("Repair.Orders", "Shop_ID");
            CreateIndex("Repair.Orders", "CreatedBy_Id");
            CreateIndex("Repair.Orders", "UpdatedBy_Id");
            CreateIndex("Repair.Photos", "CreatedBy_Id");
            CreateIndex("Repair.Photos", "UpdatedBy_Id");
            CreateIndex("Repair.Vehicles", "VehicleLookup_LookupID");
            CreateIndex("Scan.Requests", "CreatedBy_Id");
            CreateIndex("Scan.Requests", "UpdatedBy_Id");
            AddForeignKey("Repair.Orders", "Shop_ID", "Access.Shops", "ID");
            AddForeignKey("Repair.Orders", "CreatedBy_Id", "Access.Users", "UserId", cascadeDelete: true);
            AddForeignKey("Repair.Photos", "CreatedBy_Id", "Access.Users", "UserId", cascadeDelete: true);
            AddForeignKey("Repair.Photos", "UpdatedBy_Id", "Access.Users", "UserId");
            AddForeignKey("Repair.Orders", "UpdatedBy_Id", "Access.Users", "UserId");
            AddForeignKey("Repair.Vehicles", "VehicleLookup_LookupID", "Repair.VehicleLookups", "LookupID");
            AddForeignKey("Scan.Requests", "UpdatedBy_Id", "Access.Users", "UserId");
            AddForeignKey("Scan.Requests", "CreatedBy_Id", "Access.Users", "UserId", cascadeDelete: true);
            DropColumn("Repair.Photos", "Name");
            DropColumn("Repair.Photos", "UploadDt");
            DropColumn("Repair.Vehicles", "TransmissionType");
        }
        
        public override void Down()
        {
            AddColumn("Repair.Vehicles", "TransmissionType", c => c.String());
            AddColumn("Repair.Photos", "UploadDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Repair.Photos", "Name", c => c.String());
            DropForeignKey("Scan.Requests", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Scan.Requests", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Repair.Vehicles", "VehicleLookup_LookupID", "Repair.VehicleLookups");
            DropForeignKey("Repair.Orders", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Repair.Photos", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Repair.Photos", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Repair.Orders", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Repair.Orders", "Shop_ID", "Access.Shops");
            DropIndex("Scan.Requests", new[] { "UpdatedBy_Id" });
            DropIndex("Scan.Requests", new[] { "CreatedBy_Id" });
            DropIndex("Repair.Vehicles", new[] { "VehicleLookup_LookupID" });
            DropIndex("Repair.Photos", new[] { "UpdatedBy_Id" });
            DropIndex("Repair.Photos", new[] { "CreatedBy_Id" });
            DropIndex("Repair.Orders", new[] { "UpdatedBy_Id" });
            DropIndex("Repair.Orders", new[] { "CreatedBy_Id" });
            DropIndex("Repair.Orders", new[] { "Shop_ID" });
            AlterColumn("Scan.Requests", "CreatedBy_Id", c => c.String(maxLength: 128));
            AlterColumn("Scan.Requests", "CreatedDt", c => c.DateTime(nullable: false));
            AlterColumn("Repair.Vehicles", "Year", c => c.String());
            AlterColumn("Repair.Vehicles", "Model", c => c.String());
            AlterColumn("Repair.Vehicles", "Make", c => c.String());
            DropColumn("Scan.Requests", "UpdatedBy_Id");
            DropColumn("Scan.Requests", "UpdatedDt");
            DropColumn("Repair.Vehicles", "VehicleLookup_LookupID");
            DropColumn("Repair.Vehicles", "Transmission");
            DropColumn("Repair.Photos", "UpdatedBy_Id");
            DropColumn("Repair.Photos", "CreatedBy_Id");
            DropColumn("Repair.Photos", "UpdatedDt");
            DropColumn("Repair.Photos", "CreatedDt");
            DropColumn("Repair.Photos", "FileName");
            DropColumn("Repair.Orders", "UpdatedBy_Id");
            DropColumn("Repair.Orders", "CreatedBy_Id");
            DropColumn("Repair.Orders", "Shop_ID");
            DropColumn("Repair.Orders", "UpdatedDt");
            DropColumn("Repair.Orders", "CreatedDt");
            DropTable("Repair.VehicleLookups");
            CreateIndex("Scan.Requests", "CreatedBy_Id");
            AddForeignKey("Scan.Requests", "CreatedBy_Id", "Access.Users", "UserId");
        }
    }
}
