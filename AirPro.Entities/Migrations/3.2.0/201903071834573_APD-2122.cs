using System.Reflection;
using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2122 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestGridByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestsByUser");

            this.DropObjectIfExists(DropObjectType.View, "Repair", "vwOrderDetails");
            Sql(@"DROP INDEX IF EXISTS IX_RepairOrders_ShopGuid_Status ON Repair.Orders;");

            AlterColumn("Repair.Orders", "ShopReferenceNumber", c => c.String(maxLength: 500));
            AlterColumn("Repair.Orders", "InsuranceCompanyOther", c => c.String(maxLength: 200));
            AlterColumn("Repair.Orders", "InsuranceReferenceNumber", c => c.String(maxLength: 200));
            AlterColumn("Repair.Orders", "AirBagsVisualDeployments", c => c.String(maxLength: 1000));

            Sql(@"CREATE NONCLUSTERED INDEX IX_RepairOrders_ShopGuid_Status ON Repair.Orders(ShopGuid ASC, Status ASC)
	                INCLUDE(AirBagsDeployed, CCCDocumentGuid, CreatedByUserGuid, CreatedDt, DrivableInd, InsuranceCompanyId, InsuranceCompanyOther, InsuranceReferenceNumber, Odometer, ShopReferenceNumber, UpdatedByUserGuid, UpdatedDt, VehicleVIN);");

            Sql(@"ALTER TABLE [Repair].[Vehicles] DROP CONSTRAINT IF EXISTS [DF__Vehicles__Transm__778AC167]");
            Sql(@"DROP INDEX IF EXISTS IDX_VehicleVIN_VehicleMakeId_Model_Year ON Repair.Vehicles;");
            Sql(@"DROP INDEX IF EXISTS IX_VehicleVIN_VehicleMakeId_Model_Year ON Repair.Vehicles;");

            AlterColumn("Repair.Vehicles", "Make", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Repair.Vehicles", "Model", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("Repair.Vehicles", "Transmission", c => c.String(nullable: false, maxLength: 200));

            Sql(@"CREATE NONCLUSTERED INDEX IX_VehicleVIN_VehicleMakeId_Model_Year ON Repair.Vehicles (VehicleVIN ASC) INCLUDE (VehicleMakeId, Model, Year);");

            Sql(@"DROP INDEX IF EXISTS IDX_VehicleMakeId_VehicleMakeName ON Repair.VehicleMakes;");
            Sql(@"DROP INDEX IF EXISTS IX_VehicleMakeId_VehicleMakeName ON Repair.VehicleMakes;");

            AlterColumn("Repair.VehicleMakes", "VehicleMakeName", c => c.String(maxLength: 100));

            Sql(@"CREATE NONCLUSTERED INDEX IX_VehicleMakeId_VehicleMakeName ON Repair.VehicleMakes (VehicleMakeId ASC) INCLUDE (VehicleMakeName);");

            AlterColumn("Repair.InsuranceCompanies", "InsuranceCompanyName", c => c.String(maxLength: 200));

            Sql(@"ALTER TABLE Access.Shops DROP COLUMN DisplayName;");
            Sql(@"ALTER TABLE Access.Shops ADD DisplayName AS CAST(Name + ' (' + CAST(ShopId + 10000 AS NCHAR(5)) + ')' AS NVARCHAR(200));");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Views/Repair.vwOrderDetails.sql", true);
            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Views/Scan.vwRequestDetails.sql", true);

            Sql(@"DELETE FROM Access.UserPreferences WHERE ControlId = 'requests-grid';");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestGridByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestsByUser");

            this.DropObjectIfExists(DropObjectType.View, "Repair", "vwOrderDetails");
            Sql(@"DROP INDEX IF EXISTS IX_RepairOrders_ShopGuid_Status ON Repair.Orders;");

            AlterColumn("Repair.Orders", "AirBagsVisualDeployments", c => c.String());
            AlterColumn("Repair.Orders", "InsuranceReferenceNumber", c => c.String());
            AlterColumn("Repair.Orders", "InsuranceCompanyOther", c => c.String());
            AlterColumn("Repair.Orders", "ShopReferenceNumber", c => c.String());

            Sql(@"CREATE NONCLUSTERED INDEX IX_RepairOrders_ShopGuid_Status ON Repair.Orders(ShopGuid ASC, Status ASC)
	                INCLUDE(AirBagsDeployed, CCCDocumentGuid, CreatedByUserGuid, CreatedDt, DrivableInd, InsuranceCompanyId, InsuranceCompanyOther, InsuranceReferenceNumber, Odometer, ShopReferenceNumber, UpdatedByUserGuid, UpdatedDt, VehicleVIN);");

            Sql(@"ALTER TABLE [Repair].[Vehicles] DROP CONSTRAINT IF EXISTS [DF__Vehicles__Transm__778AC167]");
            Sql(@"DROP INDEX IF EXISTS IDX_VehicleVIN_VehicleMakeId_Model_Year ON Repair.Vehicles;");
            Sql(@"DROP INDEX IF EXISTS IX_VehicleVIN_VehicleMakeId_Model_Year ON Repair.Vehicles;");

            AlterColumn("Repair.Vehicles", "Transmission", c => c.String(nullable: false));
            AlterColumn("Repair.Vehicles", "Model", c => c.String(nullable: false));
            AlterColumn("Repair.Vehicles", "Make", c => c.String(nullable: false));

            Sql(@"CREATE NONCLUSTERED INDEX IX_VehicleVIN_VehicleMakeId_Model_Year ON Repair.Vehicles (VehicleVIN ASC) INCLUDE (VehicleMakeId, Model, Year);");

            Sql(@"DROP INDEX IF EXISTS IDX_VehicleMakeId_VehicleMakeName ON Repair.VehicleMakes;");
            Sql(@"DROP INDEX IF EXISTS IX_VehicleMakeId_VehicleMakeName ON Repair.VehicleMakes;");

            AlterColumn("Repair.VehicleMakes", "VehicleMakeName", c => c.String());

            Sql(@"CREATE NONCLUSTERED INDEX IX_VehicleMakeId_VehicleMakeName ON Repair.VehicleMakes (VehicleMakeId ASC) INCLUDE (VehicleMakeName);");

            AlterColumn("Repair.InsuranceCompanies", "InsuranceCompanyName", c => c.String());

            this.DropObjectIfExists(DropObjectType.View, "Scan", "vwRequestDetails");
            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Views/Repair.vwOrderDetails.sql", true);
        }
    }
}
