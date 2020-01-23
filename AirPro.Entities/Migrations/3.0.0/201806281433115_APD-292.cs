namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD292 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "AllowDemoScan", c => c.Boolean(nullable: false));

            Sql(@"SET IDENTITY_INSERT Scan.RequestTypes ON;
                    MERGE Scan.RequestTypes AS t
                    USING (SELECT 8, 'Demo Scan', 1, 1, 8, Common.udf_GetEmptyGuid(), GETUTCDATE(), 0)
	                    AS s (RequestTypeId, TypeName, ActiveFlag, BillableFlag, SortOrder, CreatedByUserGuid, CreatedDt, DefaultPrice)
                    ON t.RequestTypeId = s.RequestTypeId
                    WHEN NOT MATCHED THEN
	                    INSERT (RequestTypeId, TypeName, ActiveFlag, BillableFlag, SortOrder, CreatedByUserGuid, CreatedDt, DefaultPrice)
	                    VALUES (RequestTypeId, TypeName, ActiveFlag, BillableFlag, SortOrder, CreatedByUserGuid, CreatedDt, DefaultPrice)
                    OUTPUT inserted.*;
                SET IDENTITY_INSERT Scan.RequestTypes OFF;");
        }
        
        public override void Down()
        {
            DropColumn("Access.Shops", "AllowDemoScan");
        }
    }
}
