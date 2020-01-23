using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2123 : DbMigration
    {
        public override void Up()
        {
            Sql(@"DROP INDEX IF EXISTS IX_RepairVehicles_VehicleLookupId ON Repair.Vehicles;");
            Sql(@"CREATE NONCLUSTERED INDEX IX_RepairVehicles_VehicleLookupId ON Repair.Vehicles (VehicleLookupId) INCLUDE (VehicleMakeId, Model, Year);");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetDecodeVIN");
        }

        public override void Down()
        {
            Sql(@"DROP INDEX IF EXISTS IX_RepairVehicles_VehicleLookupId ON Repair.Vehicles;");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetDecodeVIN");
        }
    }
}
