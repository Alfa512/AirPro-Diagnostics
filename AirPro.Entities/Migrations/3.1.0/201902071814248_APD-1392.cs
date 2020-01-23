namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1392 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Technician.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId);
            
            AddColumn("Technician.Profiles", "LocationId", c => c.Int(nullable: true));
            CreateIndex("Technician.Profiles", "LocationId");
            AddForeignKey("Technician.Profiles", "LocationId", "Technician.Locations", "LocationId");

            Sql(@"SET IDENTITY_INSERT Technician.Locations ON;

		        MERGE Technician.Locations AS t
		        USING
		        (
			        SELECT
				        1 [LocationId]
				        , 'North Office' [Name]
                        , 1 [SortOrder]
                    UNION
                    
                    SELECT
				        2 [LocationId]
				        , 'South Office' [Name]
                        , 2 [SortOrder]

                    UNION

                    SELECT
				        3 [LocationId]
				        , 'Remote' [Name]
                        , 3 [SortOrder]
		        ) AS s
		        ON (t.LocationId = s.LocationId)
		        WHEN NOT MATCHED THEN
			        INSERT (LocationId, Name, SortOrder)
			        VALUES (LocationId, Name, SortOrder)
		        OUTPUT INSERTED.*;

		        SET IDENTITY_INSERT Technician.Locations OFF;

                DBCC CHECKIDENT ('Technician.Locations', RESEED);");

            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_SaveProfile");
        }
        
        public override void Down()
        {
            DropForeignKey("Technician.Profiles", "LocationId", "Technician.Locations");
            DropIndex("Technician.Profiles", new[] { "LocationId" });
            DropColumn("Technician.Profiles", "LocationId");
            DropTable("Technician.Locations");

            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_SaveProfile");
        }
    }
}
