using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class v290 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("Scan.Requests", "RepairId", "OrderId");

            DropForeignKey("Repair.Photos", "RepairOrderEntityModel_RepairOrderID", "Repair.Orders");

            RenameColumn("Repair.InsuranceCompanies", "InsuranceCompanyID", "InsuranceCompanyId");

            RenameColumn("Repair.Invoices", "InvoiceID", "InvoiceId");
            RenameColumn("Repair.Invoices", "InvoicedByID", "InvoicedByUserId");

            RenameColumn("Repair.VehicleLookups", "LookupID", "VehicleLookupId");

            RenameColumn("Repair.Vehicles", "VIN", "VehicleVIN");
            RenameColumn("Repair.Vehicles", "VehicleLookup_LookupID", "VehicleLookupId");

            RenameColumn("Repair.Orders", "RepairOrderID", "OrderId");
            RenameColumn("Repair.Orders", "InsuranceCompanyID", "InsuranceCompanyId");
            RenameColumn("Repair.Orders", "Vehicle_VIN", "VehicleVIN");

            DropTable("Repair.Photos");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612290843565_v2.9.0_Deploy.sql");

        }

        public override void Down()
        {
            RenameColumn("Scan.Requests", "OrderId", "RepairId");

            RenameColumn("Repair.InsuranceCompanies", "InsuranceCompanyId", "InsuranceCompanyID");

            RenameColumn("Repair.Invoices", "InvoiceId", "InvoiceID");
            RenameColumn("Repair.Invoices", "InvoicedByUserId", "InvoicedByID");

            RenameColumn("Repair.VehicleLookups", "VehicleLookupId", "LookupID");

            RenameColumn("Repair.Vehicles", "VehicleVIN", "VIN");
            RenameColumn("Repair.Vehicles", "VehicleLookupId", "VehicleLookup_LookupID");

            RenameColumn("Repair.Orders", "OrderId", "RepairOrderID");
            RenameColumn("Repair.Orders", "InsuranceCompanyId", "InsuranceCompanyID");
            RenameColumn("Repair.Orders", "VehicleVIN", "Vehicle_VIN");

            CreateTable(
                "Repair.Photos",
                c => new
                {
                    PhotoID = c.Int(nullable: false, identity: true),
                    FileName = c.String(),
                    ContentType = c.String(),
                    FullImage = c.Binary(),
                    ThumbnailImage = c.Binary(),
                    CreatedByUserId = c.String(nullable: false, maxLength: 128),
                    CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    UpdatedByUserId = c.String(maxLength: 128),
                    UpdatedDt = c.DateTimeOffset(precision: 7),
                    RepairOrderEntityModel_RepairOrderID = c.Int(),
                })
                .PrimaryKey(t => t.PhotoID);


            CreateIndex("Repair.Photos", "RepairOrderEntityModel_RepairOrderID");
            CreateIndex("Repair.Photos", "UpdatedByUserId");
            CreateIndex("Repair.Photos", "CreatedByUserId");

            AddForeignKey("Repair.Photos", "RepairOrderEntityModel_RepairOrderID", "Repair.Orders", "RepairOrderID");
            AddForeignKey("Repair.Photos", "UpdatedByUserId", "Access.Users", "UserId");
            AddForeignKey("Repair.Photos", "CreatedByUserId", "Access.Users", "UserId", cascadeDelete: true);

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612290843565_v2.9.0_Rollback.sql");

        }
    }
}
