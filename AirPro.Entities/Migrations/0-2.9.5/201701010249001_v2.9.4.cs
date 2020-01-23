using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v294 : DbMigration
    {
        public override void Up()
        {
            Sql(@"SELECT
                    ShopId
	                ,NEWID() [ShopGuid]
	                ,AccountGuid
	                ,Name
	                ,Phone
	                ,Fax
	                ,Address1
	                ,Address2
	                ,City
	                ,State
	                ,Zip
	                ,Notes
                INTO Access.Shops_New
                FROM Access.Shops s");

            DropForeignKey("Access.Users", "ShopId", "Access.Shops");
            DropForeignKey("Access.Shops", "AccountGuid", "Access.Accounts");
            DropForeignKey("Access.UserShops", "ShopId", "Access.Shops");
            DropForeignKey("Billing.Payments", "PaymentReceivedFromShopId", "Access.Shops");
            DropForeignKey("Repair.Orders", "ShopId", "Access.Shops");

            DropIndex("Access.Users", new[] { "ShopId" });
            DropIndex("Access.UserShops", new[] { "ShopId" });
            DropIndex("Billing.Payments", new[] { "PaymentReceivedFromShopId" });
            DropIndex("Repair.Orders", new[] { "ShopId" });

            DropPrimaryKey("Access.Shops");
            DropPrimaryKey("Access.UserShops");

            DropColumn("Access.Users", "ShopId");

            RenameColumn(table: "Access.UserShops", name: "ShopId", newName: "ShopGuid");
            RenameColumn(table: "Billing.Payments", name: "PaymentReceivedFromShopId", newName: "PaymentReceivedFromShopGuid");
            RenameColumn(table: "Repair.Orders", name: "ShopId", newName: "ShopGuid");

            AddColumn("Access.Accounts", "Phone", c => c.String());
            AddColumn("Access.Accounts", "Fax", c => c.String());

            DropTable("Access.Shops");
            RenameTable("Access.Shops_New", "Shops");

            AlterColumn("Access.Shops", "ShopGuid", c => c.Guid(nullable: false, identity: true, defaultValueSql: "NEWSEQUENTIALID()"));

            AlterColumn("Access.Shops", "AccountGuid", c => c.String());
            Sql(@"IF NOT EXISTS (SELECT 1 FROM Access.Accounts)
                    BEGIN

	                    DECLARE @CreatedByGuid UNIQUEIDENTIFIER;
	                    SELECT TOP 1 @CreatedByGuid = u.UserGuid
	                    FROM Access.Users u
	                    WHERE u.Email LIKE 'sandersmw@%'

	                    DECLARE @AccountGuid UNIQUEIDENTIFIER = NEWID();

	                    INSERT INTO Access.Accounts
	                    (
		                    AccountGuid
		                    ,Name
		                    ,Address1
		                    ,Address2
		                    ,City
		                    ,State
		                    ,Zip
		                    ,Phone
		                    ,Fax
		                    ,CreatedByUserGuid
		                    ,CreatedDt
	                    )
	                    SELECT
		                    @AccountGuid
		                    ,Name
		                    ,Address1
		                    ,Address2
		                    ,City
		                    ,State
		                    ,Zip
		                    ,Phone
		                    ,Fax
		                    ,@CreatedByGuid
		                    ,GETUTCDATE()
	                    FROM Access.Shops s
	                    WHERE s.ShopId = 1

	                    UPDATE s
		                    SET s.AccountGuid = @AccountGuid
	                    FROM Access.Shops s
	                    WHERE s.AccountGuid IS NULL

                    END");
            AlterColumn("Access.Shops", "AccountGuid", c => c.Guid(nullable: false));

            AlterColumn("Access.UserShops", "ShopGuid", c => c.String());
            Sql(@"UPDATE us
	                SET us.ShopGuid = s.ShopGuid
                FROM Access.UserShops us
                INNER JOIN Access.Shops s
	                ON us.ShopGuid = s.ShopId");
            AlterColumn("Access.UserShops", "ShopGuid", c => c.Guid(nullable: false));

            AlterColumn("Billing.Payments", "PaymentReceivedFromShopGuid", c => c.String());
            Sql(@"UPDATE bp
	                SET bp.PaymentReceivedFromShopGuid = s.ShopGuid
                FROM Billing.Payments bp
                INNER JOIN Access.Shops s
	                ON bp.PaymentReceivedFromShopGuid = s.ShopId");
            AlterColumn("Billing.Payments", "PaymentReceivedFromShopGuid", c => c.Guid(nullable: false));

            AlterColumn("Repair.Orders", "ShopGuid", c => c.String());
            Sql(@"UPDATE o
	                SET o.ShopGuid = s.ShopGuid
                FROM Repair.Orders o
                INNER JOIN Access.Shops s
	                ON o.ShopGuid = s.ShopId");
            AlterColumn("Repair.Orders", "ShopGuid", c => c.Guid(nullable: false));

            AddPrimaryKey("Access.Shops", "ShopGuid");
            AddPrimaryKey("Access.UserShops", new[] { "UserGuid", "ShopGuid" });

            CreateIndex("Access.UserShops", "ShopGuid");
            CreateIndex("Access.Shops", "AccountGuid");
            CreateIndex("Billing.Payments", "PaymentReceivedFromShopGuid");
            CreateIndex("Repair.Orders", "ShopGuid");

            AddForeignKey("Access.Shops", "AccountGuid", "Access.Accounts", "AccountGuid");
            AddForeignKey("Access.UserShops", "ShopGuid", "Access.Shops", "ShopGuid");
            AddForeignKey("Billing.Payments", "PaymentReceivedFromShopGuid", "Access.Shops", "ShopGuid");
            AddForeignKey("Repair.Orders", "ShopGuid", "Access.Shops", "ShopGuid");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201701010249001_v2.9.4_Deploy.sql");

        }

        public override void Down()
        {
            DropForeignKey("Repair.Orders", "ShopGuid", "Access.Shops");
            DropForeignKey("Billing.Payments", "PaymentReceivedFromShopGuid", "Access.Shops");
            DropForeignKey("Access.UserShops", "ShopGuid", "Access.Shops");
            DropForeignKey("Access.Shops", "AccountGuid", "Access.Accounts");

            DropIndex("Repair.Orders", new[] { "ShopGuid" });
            DropIndex("Billing.Payments", new[] { "PaymentReceivedFromShopGuid" });
            DropIndex("Access.Shops", new[] { "AccountGuid" });
            DropIndex("Access.UserShops", new[] { "ShopGuid" });

            DropPrimaryKey("Access.UserShops");
            DropPrimaryKey("Access.Shops");

            AlterColumn("Repair.Orders", "ShopGuid", c => c.String());
            Sql(@"UPDATE o
	                SET o.ShopGuid = s.ShopId
                FROM Repair.Orders o
                INNER JOIN Access.Shops s
	                ON o.ShopGuid = s.ShopGuid");
            AlterColumn("Repair.Orders", "ShopGuid", c => c.Int(nullable: false));

            AlterColumn("Billing.Payments", "PaymentReceivedFromShopGuid", c => c.String());
            Sql(@"UPDATE bp
	                SET bp.PaymentReceivedFromShopGuid = s.ShopId
                FROM Billing.Payments bp
                INNER JOIN Access.Shops s
	                ON bp.PaymentReceivedFromShopGuid = s.ShopGuid");
            AlterColumn("Billing.Payments", "PaymentReceivedFromShopGuid", c => c.Int(nullable: false));

            AlterColumn("Access.UserShops", "ShopGuid", c => c.String());
            Sql(@"UPDATE us
	                SET us.ShopGuid = s.ShopId
                FROM Access.UserShops us
                INNER JOIN Access.Shops s
	                ON us.ShopGuid = s.ShopGuid");
            AlterColumn("Access.UserShops", "ShopGuid", c => c.Int(nullable: false));

            AlterColumn("Access.Shops", "AccountGuid", c => c.Guid());

            DropColumn("Access.Shops", "ShopGuid");
            DropColumn("Access.Accounts", "Fax");
            DropColumn("Access.Accounts", "Phone");

            RenameColumn(table: "Access.UserShops", name: "ShopGuid", newName: "ShopId");
            RenameColumn(table: "Repair.Orders", name: "ShopGuid", newName: "ShopId");
            RenameColumn(table: "Billing.Payments", name: "PaymentReceivedFromShopGuid", newName: "PaymentReceivedFromShopId");

            AddColumn("Access.Users", "ShopId", c => c.String());
            Sql(@"UPDATE u
	                SET u.ShopId = us.ShopId
                FROM Access.Users u
                INNER JOIN Access.UserShops us
	                ON u.UserGuid = us.UserGuid");
            AlterColumn("Access.Users", "ShopId", c => c.Int(nullable: false));

            AddPrimaryKey("Access.UserShops", new[] { "UserGuid", "ShopId" });
            AddPrimaryKey("Access.Shops", "ShopId");

            CreateIndex("Repair.Orders", "ShopId");
            CreateIndex("Billing.Payments", "PaymentReceivedFromShopId");
            CreateIndex("Access.UserShops", "ShopId");
            CreateIndex("Access.Shops", "AccountGuid");
            CreateIndex("Access.Users", "ShopId");

            AddForeignKey("Repair.Orders", "ShopId", "Access.Shops", "ShopId");
            AddForeignKey("Billing.Payments", "PaymentReceivedFromShopId", "Access.Shops", "ShopId");
            AddForeignKey("Access.UserShops", "ShopId", "Access.Shops", "ShopId");
            AddForeignKey("Access.Shops", "AccountGuid", "Access.Accounts", "AccountGuid");
            AddForeignKey("Access.Users", "ShopId", "Access.Shops", "ShopId");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201701010249001_v2.9.4_Rollback.sql");

        }
    }
}
