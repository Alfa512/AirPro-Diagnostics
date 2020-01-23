namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1323 : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT Scan.RequestTypes ON;

		        MERGE Scan.RequestTypes AS t
		        USING
		        (
			        SELECT
				        10 [RequestTypeId]
				        ,'Programming Scan' [TypeName]
				        ,1 [ActiveFlag]
				        ,1 [BillableFlag]
				        ,(SELECT MAX(SortOrder) + 1 FROM [Scan].[RequestTypes]) [SortOrder]
		        ) AS s
		        ON (t.RequestTypeId = s.RequestTypeId)
		        WHEN NOT MATCHED THEN
			        INSERT (RequestTypeId, TypeName, ActiveFlag, BillableFlag, SortOrder, CreatedByUserGuid, CreatedDt)
			        VALUES (RequestTypeId, TypeName, 1, 1, SortOrder, Common.udf_GetEmptyGuid(), GETUTCDATE())
		        OUTPUT INSERTED.*;

		        SET IDENTITY_INSERT Scan.RequestTypes OFF;

                DBCC CHECKIDENT ('Scan.RequestTypes', RESEED);");
        }

        public override void Down()
        {
            Sql(@"DELETE FROM [Scan].[RequestTypes] WHERE RequestTypeId = 10;
                DBCC CHECKIDENT ('Scan.RequestTypes', RESEED);");
        }
    }
}
