namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v313 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "AllowSelfScan", c => c.Boolean(nullable: false));

            Sql(@"IF (SELECT MAX(RequestTypeId) FROM Scan.RequestTypes) = 5

                BEGIN

                    SET IDENTITY_INSERT Scan.RequestTypes ON

                    INSERT INTO Scan.RequestTypes
                    (
	                    RequestTypeId
	                    ,TypeName
	                    ,ActiveFlag
	                    ,BillableFlag
	                    ,SortOrder
	                    ,CreatedByUserGuid
	                    ,CreatedDt
                    )
                    SELECT
	                    n.RequestTypeId
	                    ,n.TypeName
	                    ,n.ActiveFlag
	                    ,n.BillableFlag
	                    ,n.SortOrder
	                    ,n.CreatedByUserGuid
	                    ,n.CreateDt
                    FROM
                    (
	                    SELECT
	                    '6' [RequestTypeId]
	                    ,'Self Scan' [TypeName]
	                    ,0 [ActiveFlag]
	                    ,0 [BillableFlag]
	                    ,6 [SortOrder]
	                    ,'00000000-0000-0000-0000-000000000000' [CreatedByUserGuid]
	                    ,GETUTCDATE() [CreateDt]
                    ) n
                    LEFT JOIN Scan.RequestTypes rt
	                    ON n.RequestTypeId = rt.RequestTypeId
                    WHERE rt.RequestTypeId IS NULL

                    SET IDENTITY_INSERT Scan.RequestTypes OFF

                END");
        }
        
        public override void Down()
        {
            DropColumn("Access.Shops", "AllowSelfScan");
        }
    }
}
