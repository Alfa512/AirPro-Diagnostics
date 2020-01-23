namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1484 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Billing.Cycles", "SortOrder", c => c.Int(nullable: false));
            Sql(@"SET IDENTITY_INSERT Billing.Cycles ON;

		        MERGE Billing.Cycles AS t
		        USING
		        (
			        SELECT
				        4 [CycleId]
				        ,'Daily' [CycleName]
                        ,-1 [SortOrder]
		        ) AS s
		        ON (t.CycleId = s.CycleId)
		        WHEN NOT MATCHED THEN
			        INSERT (CycleId, CycleName, SortOrder)
			        VALUES (CycleId, CycleName, SortOrder)
		        OUTPUT INSERTED.*;

		        SET IDENTITY_INSERT Billing.Cycles OFF;

                DBCC CHECKIDENT ('Billing.Cycles', RESEED);");
        }
        
        public override void Down()
        {
            DropColumn("Billing.Cycles", "SortOrder");
        }
    }
}
