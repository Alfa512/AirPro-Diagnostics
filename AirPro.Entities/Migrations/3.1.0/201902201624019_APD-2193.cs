namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2193 : DbMigration
    {
        public override void Up()
        {
            Sql(@"MERGE Scan.WorkTypeVehicleMakes AS t
                USING
                (
	                SELECT
		                wt.WorkTypeId
		                ,vm.VehicleMakeId
	                FROM Repair.VehicleMakes vm, Scan.WorkTypes wt
	                INNER JOIN Scan.WorkTypeGroups wtg
		                ON wt.WorkTypeGroupId = wtg.WorkTypeGroupId
	                WHERE wt.WorkTypeActiveInd = 1
		                AND wtg.WorkTypeGroupActiveInd = 1
                ) AS s
                ON (t.WorkTypeId = s.WorkTypeId AND t.VehicleMakeId = s.VehicleMakeId)
                WHEN NOT MATCHED THEN
	                INSERT (WorkTypeId, VehicleMakeId)
	                VALUES (WorkTypeId, VehicleMakeId)
                OUTPUT INSERTED.*;");
        }
        
        public override void Down()
        {
        }
    }
}
