namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD208 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Access' AND TABLE_NAME = 'vwUserMemberships') DROP VIEW Access.vwUserMemberships;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetRequestGridByUser') DROP PROCEDURE Scan.usp_GetRequestGridByUser;");

            Sql(@"CREATE INDEX IDX_OrderId_ShopGuid_VehicleVIN ON Repair.Orders (OrderId) INCLUDE (ShopGuid, VehicleVIN);");
            Sql(@"CREATE INDEX IDX_ReportId_CompletedInd_ResponsibleTech ON Scan.Reports (ReportId) INCLUDE (CompletedInd, ResponsibleTechnicianUserGuid);");
            Sql(@"CREATE INDEX IDX_RequestId_RequestTypeId_OrderId_ReportId_CreatedDt ON Scan.Requests (RequestId) INCLUDE (RequestTypeId, OrderId, ReportId, CreatedDt);");
            Sql(@"CREATE INDEX IDX_RequestId_CreatedDt ON Scan.UploadXmls (RequestId ASC, CreatedDt DESC);");

            Sql(@"CREATE INDEX IDX_UserGuid_LastName_FirstName ON Access.Users (UserGuid) INCLUDE (LastName, FirstName);");
            Sql(@"CREATE INDEX IDX_ShopGuid_ShopName ON Access.Shops (ShopGuid) INCLUDE (Name);");
            Sql(@"CREATE INDEX IDX_RequestTypeId_TypeName ON Scan.RequestTypes (RequestTypeId) INCLUDE (TypeName);");

            Sql(@"CREATE INDEX IDX_VehicleVIN_VehicleMakeId_Model_Year ON Repair.Vehicles (VehicleVIN) INCLUDE (VehicleMakeId, Model, Year);");
            Sql(@"CREATE INDEX IDX_VehicleMakeId_VehicleMakeName ON Repair.VehicleMakes (VehicleMakeId) INCLUDE (VehicleMakeName);");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Access' AND TABLE_NAME = 'vwUserMemberships') DROP VIEW Access.vwUserMemberships;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetRequestGridByUser') DROP PROCEDURE Scan.usp_GetRequestGridByUser;");

            Sql(@"DROP INDEX IDX_OrderId_ShopGuid_VehicleVIN ON Repair.Orders;");
            Sql(@"DROP INDEX IDX_ReportId_CompletedInd_ResponsibleTech ON Scan.Reports;");
            Sql(@"DROP INDEX IDX_RequestId_RequestTypeId_OrderId_ReportId_CreatedDt ON Scan.Requests;");
            Sql(@"DROP INDEX IDX_RequestId_CreatedDt ON Scan.UploadXmls;");

            Sql(@"DROP INDEX IDX_UserGuid_LastName_FirstName ON Access.Users;");
            Sql(@"DROP INDEX IDX_ShopGuid_ShopName ON Access.Shops;");
            Sql(@"DROP INDEX IDX_RequestTypeId_TypeName ON Scan.RequestTypes;");

            Sql(@"DROP INDEX IDX_VehicleVIN_VehicleMakeId_Model_Year ON Repair.Vehicles;");
            Sql(@"DROP INDEX IDX_VehicleMakeId_VehicleMakeName ON Repair.VehicleMakes;");
        }
    }
}
