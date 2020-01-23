namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v317 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Repair.VehicleMakes",
                c => new
                {
                    VehicleMakeId = c.Int(nullable: false, identity: true),
                    VehicleMakeName = c.String(),
                    VehicleMakeTypeId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.VehicleMakeId)
                .ForeignKey("Repair.VehicleMakeTypes", t => t.VehicleMakeTypeId)
                .Index(t => t.VehicleMakeTypeId);

            CreateTable(
                "Repair.VehicleMakeTypes",
                c => new
                {
                    VehicleMakeTypeId = c.Int(nullable: false, identity: true),
                    VehicleMakeTypeName = c.String(),
                })
                .PrimaryKey(t => t.VehicleMakeTypeId);

            AddColumn("Repair.Vehicles", "VehicleMakeId", c => c.Int(nullable: true));

            Sql(@"SET XACT_ABORT ON;
                BEGIN TRAN

	                BEGIN TRY

		                IF NOT EXISTS (SELECT 1 FROM Repair.VehicleMakeTypes)
		                BEGIN
			                SET IDENTITY_INSERT Repair.VehicleMakeTypes ON;
			                INSERT INTO Repair.VehicleMakeTypes (VehicleMakeTypeId, VehicleMakeTypeName)
			                SELECT 1, 'Domestic' UNION SELECT 2, 'European' UNION SELECT 3, 'Asian'
			                SET IDENTITY_INSERT Repair.VehicleMakeTypes OFF;
		                END

		                INSERT INTO Repair.VehicleMakes (VehicleMakeTypeId, VehicleMakeName)
		                SELECT
			                CASE
				                WHEN RTRIM(LTRIM(Make)) IN ('Buick','Cadillac','Chevrolet','Chrysler','Dodge','Ram','Ford','Lincoln','Mercury','Oldsmobile','GMC','Jeep','HUMMER','Pontiac','Saturn','Tesla') THEN 1
				                WHEN RTRIM(LTRIM(Make)) IN ('Alfa Romeo','Audi','BMW','Ferrari','FIAT','Mercedes-Benz','Maserati','MINI','Porsche','Saab','smart','Volkswagen','Volvo','Land Rover','Jaguar') THEN 2
				                WHEN RTRIM(LTRIM(Make)) IN ('Acura','Honda','Hyundai','INFINITI','Genesis','Mazda','Nissan','Mitsubishi','Toyota','Scion','Subaru','Suzuki','Kia','Lexus') THEN 3
			                END [VehicleMakeTypeId]
			                ,RTRIM(LTRIM(Make)) [VehicleMakeName]
		                FROM Repair.Vehicles v
		                WHERE v.VehicleLookupId IS NOT NULL
			                AND NOT EXISTS (SELECT 1 FROM Repair.VehicleMakes WHERE VehicleMakeName = RTRIM(LTRIM(v.Make)))
		                GROUP BY Make
		                ORDER BY Make

		                INSERT Repair.VehicleMakes (VehicleMakeTypeId, VehicleMakeName)
		                SELECT 1, 'Other (Domestic)' WHERE NOT EXISTS (SELECT 1 FROM Repair.VehicleMakes WHERE VehicleMakeName = 'Other (Domestic)') UNION
		                SELECT 2, 'Other (European)' WHERE NOT EXISTS (SELECT 1 FROM Repair.VehicleMakes WHERE VehicleMakeName = 'Other (European)') UNION
		                SELECT 3, 'Other (Asian)' WHERE NOT EXISTS (SELECT 1 FROM Repair.VehicleMakes WHERE VehicleMakeName = 'Other (Asian)')

		                IF (OBJECT_ID('tempdb..#Mapping') IS NOT NULL) DROP TABLE #Mapping;

		                CREATE TABLE #Mapping (VehicleVIN CHAR(17), VehicleMakeId INT)

		                INSERT INTO #Mapping (VehicleVIN, VehicleMakeId)
		                SELECT v.VehicleVIN, n.VehicleMakeId
		                FROM Repair.Vehicles v
		                OUTER APPLY
		                (
			                SELECT TOP 1 m.VehicleMakeId
			                FROM Repair.VehicleMakes m
			                WHERE RTRIM(LTRIM(v.Make)) = RTRIM(LTRIM(m.VehicleMakeName))
				                OR ((v.Make LIKE '%Benz%' OR v.Make LIKE '%mbz%' OR v.Make = 'Mercedes') AND m.VehicleMakeName = 'Mercedes-Benz')
				                OR (v.Make LIKE '%Homda%' AND m.VehicleMakeName = 'Honda')
				                OR (v.Make LIKE 'tyo%' AND m.VehicleMakeName = 'Toyota')
				                OR RTRIM(LTRIM(v.Make)) = LEFT(RTRIM(LTRIM(m.VehicleMakeName)), 4)
		                ) n
		                WHERE v.VehicleMakeId IS NULL

		                UPDATE mp
			                SET mp.VehicleVIN = v.VehicleVIN
				                ,mp.VehicleMakeId = m.VehicleMakeId
		                FROM #Mapping mp
		                INNER JOIN Repair.Vehicles v
			                ON mp.VehicleVIN = v.VehicleVIN
		                OUTER APPLY
		                (
			                SELECT TOP 1 m.VehicleMakeId
			                FROM Repair.VehicleMakes m
			                WHERE RTRIM(LTRIM(v.Model)) =  RTRIM(LTRIM(m.VehicleMakeName))
		                ) m
		                WHERE mp.VehicleMakeId IS NULL
			                AND m.VehicleMakeId IS NOT NULL

		                UPDATE mp
			                SET mp.VehicleVIN = v.VehicleVIN
				                ,mp.VehicleMakeId = m.VehicleMakeId
		                FROM #Mapping mp
		                INNER JOIN Repair.Vehicles v
			                ON mp.VehicleVIN = v.VehicleVIN
		                OUTER APPLY
		                (
			                SELECT TOP 1 m.VehicleMakeId
			                FROM Repair.VehicleMakes m
			                WHERE LEFT(RTRIM(LTRIM(m.VehicleMakeName)), 4) = LEFT(RTRIM(LTRIM(v.Make)), 4)
				                OR LEFT(RTRIM(LTRIM(m.VehicleMakeName)), 4) = LEFT(RTRIM(LTRIM(v.Model)), 4)
		                ) m
		                WHERE mp.VehicleMakeId IS NULL
			                AND m.VehicleMakeId IS NOT NULL

		                DECLARE @MakeId INT = (SELECT TOP 1 VehicleMakeId FROM Repair.VehicleMakes WHERE VehicleMakeName = 'Other (Domestic)')

		                UPDATE mp
			                SET mp.VehicleMakeId = @MakeId
		                FROM #Mapping mp
		                WHERE mp.VehicleMakeId IS NULL

		                UPDATE v
			                SET v.VehicleMakeId = m.VehicleMakeId
		                FROM Repair.Vehicles v
		                INNER JOIN #Mapping m
			                ON v.VehicleVIN = m.VehicleVIN
	                END TRY
	                BEGIN CATCH
		                IF @@TRANCOUNT > 0 ROLLBACK TRAN;
		                THROW;
	                END CATCH
                IF @@TRANCOUNT > 0 COMMIT TRAN;");

            AlterColumn("Repair.Vehicles", "VehicleMakeId", c => c.Int(nullable: false));
            CreateIndex("Repair.Vehicles", "VehicleMakeId");
            AddForeignKey("Repair.Vehicles", "VehicleMakeId", "Repair.VehicleMakes", "VehicleMakeId");
        }

        public override void Down()
        {
            DropForeignKey("Repair.Vehicles", "VehicleMakeId", "Repair.VehicleMakes");
            DropForeignKey("Repair.VehicleMakes", "VehicleMakeTypeId", "Repair.VehicleMakeTypes");
            DropIndex("Repair.VehicleMakes", new[] { "VehicleMakeTypeId" });
            DropIndex("Repair.Vehicles", new[] { "VehicleMakeId" });
            DropColumn("Repair.Vehicles", "VehicleMakeId");
            DropTable("Repair.VehicleMakeTypes");
            DropTable("Repair.VehicleMakes");
        }
    }
}
