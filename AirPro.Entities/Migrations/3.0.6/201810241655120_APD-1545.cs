namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1545 : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT Billing.Cycles ON;

		        MERGE Billing.Cycles AS t
		        USING
		        (
			        SELECT
				        5 [CycleId]
				        ,'Bi-Monthly' [CycleName]
                        ,5 [SortOrder]
		        ) AS s
		        ON (t.CycleId = s.CycleId)
		        WHEN NOT MATCHED THEN
			        INSERT (CycleId, CycleName, SortOrder)
			        VALUES (CycleId, CycleName, SortOrder)
		        OUTPUT INSERTED.*;

		        SET IDENTITY_INSERT Billing.Cycles OFF;

                DBCC CHECKIDENT ('Billing.Cycles', RESEED);

                UPDATE c
	                SET c.SortOrder = o.SortOrder
                FROM Billing.Cycles c
                INNER JOIN
                (
	                SELECT
		                CycleId
		                ,ROW_NUMBER() OVER (ORDER BY CycleName) [SortOrder]
	                FROM Billing.Cycles
                ) o ON c.CycleId = o.CycleId;");
        }

        public override void Down()
        {
            Sql("DELETE FROM Billing.Cycles WHERE CycleId = 5");
        }
    }
}
