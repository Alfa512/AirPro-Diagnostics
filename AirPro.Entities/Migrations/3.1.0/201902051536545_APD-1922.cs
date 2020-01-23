namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1922 : DbMigration
    {
        public override void Up()
        {
            Sql(@"MERGE Repair.Vehicles AS t
                USING
                (
	                SELECT
		                'AP000000000000000' [VehicleVIN]
		                ,'Other (Domestic)' [Make]
		                ,'Unknown' [Model]
		                ,'9999' [Year]
		                ,'Unknown' [Transmission]
		                ,(SELECT VehicleMakeId FROM Repair.VehicleMakes WHERE VehicleMakeName = 'Other (Domestic)') [VehicleMakeId]
                ) AS s
                ON (t.VehicleVIN = s.VehicleVIN)
                WHEN NOT MATCHED THEN
	                INSERT (VehicleVIN, Make, Model, Year, Transmission, VehicleMakeId)
	                VALUES (VehicleVIN, Make, Model, Year, Transmission, VehicleMakeId)
                WHEN MATCHED THEN
	                UPDATE
		                SET t.Make = s.Make
			                ,t.Model = s.Model
			                ,t.Year = s.Year
			                ,t.Transmission = s.Transmission
			                ,t.VehicleMakeId = s.VehicleMakeId
                OUTPUT INSERTED.*;");
        }
        
        public override void Down()
        {
        }
    }
}
