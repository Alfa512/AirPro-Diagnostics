namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v295 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS (SELECT 1 FROM Access.Users WHERE UserName = 'system@airprodiag.com')
	            INSERT INTO Access.Users (UserGuid, UserName, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
	            VALUES ('00000000-0000-0000-0000-000000000000', 'system@airprodiag.com', 'system@airprodiag.com', 0, 0, 0, 1, 0)");

            DropForeignKey("Access.UserShops", "ShopGuid", "Access.Shops");
            DropForeignKey("Billing.Payments", "PaymentReceivedFromShopGuid", "Access.Shops");
            DropForeignKey("Repair.Orders", "ShopGuid", "Access.Shops");

            DropIndex("Access.Shops", new[] { "AccountGuid" });

            DropPrimaryKey("Access.Shops");

            DropForeignKey("Access.UserRoles", "RoleGuid", "Access.Roles");
            DropForeignKey("Access.GroupRoles", "RoleGuid", "Access.Roles");

            DropPrimaryKey("Access.UserRoles");
            DropPrimaryKey("Access.Roles");

            Sql(@"UPDATE ur
	                SET ur.RoleGuid = u.RoleGuid
                FROM Access.Roles r
                INNER JOIN Access.UserRoles ur
	                ON r.RoleGuid = ur.RoleGuid
                INNER JOIN
                (
	                SELECT 'Administrator' [Role], '4FD55BFD-9E79-48C8-9FB0-DB5281D5005F' [RoleGuid] UNION ALL
	                SELECT 'Technician', '89CC2BF0-62BB-435B-8BD8-3243BE4EABE9' UNION ALL
	                SELECT 'Client', '4BDC867F-5801-424F-8617-2D2D6BA7C6EF' UNION ALL
	                SELECT 'Billing', '7A5716F7-0908-45FA-879D-1B5F9362FA71' UNION ALL
	                SELECT 'Developer', '4C1F436B-86C5-4F7B-83DA-6DE88E72EF05'
                ) u
	                ON r.Name = u.[Role]

                UPDATE r
	                SET r.RoleGuid = u.RoleGuid
                FROM Access.Roles r
                INNER JOIN
                (
	                SELECT 'Administrator' [Role], '4FD55BFD-9E79-48C8-9FB0-DB5281D5005F' [RoleGuid] UNION ALL
	                SELECT 'Technician', '89CC2BF0-62BB-435B-8BD8-3243BE4EABE9' UNION ALL
	                SELECT 'Client', '4BDC867F-5801-424F-8617-2D2D6BA7C6EF' UNION ALL
	                SELECT 'Billing', '7A5716F7-0908-45FA-879D-1B5F9362FA71' UNION ALL
	                SELECT 'Developer', '4C1F436B-86C5-4F7B-83DA-6DE88E72EF05'
                ) u
	                ON r.Name = u.[Role]");

            AddPrimaryKey("Access.Roles", "RoleGuid");

            AddPrimaryKey("Access.UserRoles", new[] { "UserGuid", "RoleGuid" });
            AddForeignKey("Access.UserRoles", "RoleGuid", "Access.Roles");
            AddForeignKey("Access.GroupRoles", "RoleGuid", "Access.Roles");

            AlterColumn("Access.Accounts", "AccountGuid", c => c.Guid(nullable: false, identity: true, defaultValueSql: "NEWSEQUENTIALID()"));
            AlterColumn("Access.Shops", "ShopGuid", c => c.Guid(nullable: false, identity: true, defaultValueSql: "NEWSEQUENTIALID()"));
            AlterColumn("Access.Shops", "AccountGuid", c => c.Guid(nullable: false));

            AddPrimaryKey("Access.Shops", "ShopGuid");

            CreateIndex("Access.Shops", "AccountGuid");

            AddForeignKey("Access.UserShops", "ShopGuid", "Access.Shops", "ShopGuid");
            AddForeignKey("Billing.Payments", "PaymentReceivedFromShopGuid", "Access.Shops", "ShopGuid");
            AddForeignKey("Repair.Orders", "ShopGuid", "Access.Shops", "ShopGuid");

            DropColumn("Access.Users", "AllowAllUsers");
            DropColumn("Access.Users", "AllowAllShops");
            DropColumn("Access.Users", "AllowAllAccounts");

            // Setup Default Groups.
            Sql(@"DECLARE @CreatedByGuid UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';

                INSERT INTO Access.Groups (GroupGuid, Name, CreatedByUserGuid, CreatedDt)
                SELECT
	                NEWID()
	                ,n.Name
	                ,@CreatedByGuid
	                ,GETUTCDATE()
                FROM
                (
	                SELECT 'Administrator' [Name] UNION ALL
	                SELECT 'Billing' UNION ALL
	                SELECT 'Client' UNION ALL
	                SELECT 'Technician' UNION ALL
	                SELECT 'Developer'
                ) n
                LEFT JOIN Access.Groups g
	                ON n.Name = g.Name
                WHERE g.GroupGuid IS NULL

                INSERT INTO Access.GroupRoles (GroupGuid, RoleGuid, CreatedByUserGuid, CreatedDt)
                SELECT
	                n.GroupGuid
	                ,n.RoleGuid
	                ,@CreatedByGuid
	                ,GETUTCDATE()
                FROM
                (
	                SELECT
		                g.GroupGuid
		                ,r.RoleGuid
	                FROM Access.Groups g
	                INNER JOIN Access.Roles r
		                ON g.Name = r.Name
                ) n
                LEFT JOIN Access.GroupRoles gr
	                ON n.GroupGuid = gr.GroupGuid
		                AND n.RoleGuid = gr.RoleGuid
                WHERE gr.GroupGuid IS NULL

                INSERT INTO Access.UserGroups (UserGuid, GroupGuid, CreatedByUserGuid, CreatedDt)
                SELECT
	                n.UserGuid
	                ,n.GroupGuid
	                ,@CreatedByGuid
	                ,GETUTCDATE()
                FROM
                (
	                SELECT
		                ur.UserGuid
		                ,gr.GroupGuid
	                FROM Access.GroupRoles gr
	                INNER JOIN Access.UserRoles ur
		                ON gr.RoleGuid = ur.RoleGuid
	                GROUP BY
		                ur.UserGuid
		                ,gr.GroupGuid
                ) n
                LEFT JOIN Access.UserGroups u
	                ON n.UserGuid = u.UserGuid
		                AND n.GroupGuid = u.GroupGuid
                WHERE u.GroupGuid IS NULL");
        }
        
        public override void Down()
        {
            AddColumn("Access.Users", "AllowAllAccounts", c => c.Boolean(nullable: false));
            AddColumn("Access.Users", "AllowAllShops", c => c.Boolean(nullable: false));
            AddColumn("Access.Users", "AllowAllUsers", c => c.Boolean(nullable: false));
            DropForeignKey("Repair.Orders", "ShopGuid", "Access.Shops");
            DropForeignKey("Billing.Payments", "PaymentReceivedFromShopGuid", "Access.Shops");
            DropForeignKey("Access.UserShops", "ShopGuid", "Access.Shops");
            DropIndex("Access.Shops", new[] { "AccountGuid" });
            DropPrimaryKey("Access.Shops");
            AlterColumn("Access.Shops", "AccountGuid", c => c.Guid(nullable: false, identity: true));
            AlterColumn("Access.Shops", "ShopGuid", c => c.Guid(nullable: false));
            AddPrimaryKey("Access.Shops", "ShopGuid");
            CreateIndex("Access.Shops", "AccountGuid");
            AddForeignKey("Repair.Orders", "ShopGuid", "Access.Shops", "ShopGuid");
            AddForeignKey("Billing.Payments", "PaymentReceivedFromShopGuid", "Access.Shops", "ShopGuid");
            AddForeignKey("Access.UserShops", "ShopGuid", "Access.Shops", "ShopGuid");
        }
    }
}
