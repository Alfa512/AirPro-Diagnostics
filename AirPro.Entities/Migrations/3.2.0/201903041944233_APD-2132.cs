using System.Reflection;
using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2132 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "Repair.OrderStatuses",
                    c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        StatusName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StatusId);

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetAgingRepairsByUser");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Views/Repair.vwOrderDetails.sql", true);

            Sql(@"IF NOT EXISTS (SELECT 1 FROM sys.sysfulltextcatalogs WHERE name = 'AirProSearchCatalog')
	                CREATE FULLTEXT CATALOG AirProSearchCatalog AS DEFAULT;");

            Sql(@"DROP INDEX IF EXISTS [nci_wi_Orders_ABA3C5A2486CFBAF5BBDD35B5948B70C] ON [Repair].[Orders];");

            Sql(@"DROP INDEX IF EXISTS IX_RepairOrders_ShopGuid_Status ON [Repair].[Orders];");
            Sql(@"CREATE INDEX IX_RepairOrders_ShopGuid_Status ON [Repair].[Orders]([ShopGuid] ASC, [Status] ASC)
                    INCLUDE([AirBagsDeployed], [CCCDocumentGuid], [CreatedByUserGuid], [CreatedDt], [DrivableInd], [InsuranceCompanyId], [InsuranceCompanyOther], [InsuranceReferenceNumber], [Odometer], [ShopReferenceNumber], [UpdatedByUserGuid], [UpdatedDt], [VehicleVIN]);");

            Sql(@"DROP INDEX IF EXISTS IX_ScanReports_CompletedInd ON Scan.Reports;");
            Sql(@"CREATE INDEX IX_ScanReports_CompletedInd ON Scan.Reports (CompletedInd);");

            Sql(@"DROP INDEX IF EXISTS IX_CommonUploads_UploadDeletedInd ON [Common].[Uploads];");
            Sql(@"CREATE INDEX IX_CommonUploads_UploadDeletedInd ON [Common].[Uploads] ([UploadDeletedInd]) INCLUDE ([UploadKey],[UploadTypeId],[UploadFileName],[UploadFileExtension],[CreatedDt]);");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetAgingRepairsByUser");

            this.DropObjectIfExists(DropObjectType.View, "Repair", "vwOrderDetails");

            DropTable("Repair.OrderStatuses");

            Sql(@"DROP INDEX IF EXISTS IX_ScanReports_CompletedInd ON Scan.Reports;");

            Sql(@"DROP INDEX IF EXISTS IX_CommonUploads_UploadDeletedInd ON [Common].[Uploads];");
        }
    }
}
