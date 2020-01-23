using System.Data.SqlClient;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v286 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Access.UserClaims", name: "Id", newName: "ClaimId");
            RenameColumn(table: "Access.Roles", name: "Id", newName: "RoleId");
            RenameColumn(table: "Access.Logins", name: "LoginID", newName: "LoginId");

            DropForeignKey("Access.Logins", "UserID", "Access.Users");
            DropIndex("Access.Logins", new[] { "UserID" });
            RenameColumn(table: "Access.Logins", name: "UserID", newName: "UserId");
            CreateIndex("Access.Logins", new[] { "UserId" });
            AddForeignKey("Access.Logins", "UserId", "Access.Users", "UserId");

            CreateTable(
                "Access.Accounts",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedBy_Id = c.String(maxLength: 128),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            CreateTable(
                "Access.GroupRoles",
                c => new
                    {
                        GroupId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedBy_Id = c.String(maxLength: 128),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => new { t.GroupId, t.RoleId })
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Access.Groups", t => t.GroupId)
                .ForeignKey("Access.Roles", t => t.RoleId)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            CreateTable(
                "Access.Groups",
                c => new
                    {
                        GroupId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedBy_Id = c.String(maxLength: 128),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "Access.UserAccounts",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        AccountId = c.String(nullable: false, maxLength: 128),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedBy_Id = c.String(maxLength: 128),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.AccountId })
                .ForeignKey("Access.Accounts", t => t.AccountId)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .ForeignKey("Access.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AccountId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            CreateTable(
                "Access.UserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.String(nullable: false, maxLength: 128),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedBy_Id = c.String(maxLength: 128),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("Access.Groups", t => t.GroupId)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .ForeignKey("Access.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.GroupId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            CreateTable(
                "Access.UserShops",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.Int(nullable: false),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedBy_Id = c.String(maxLength: 128),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ShopId })
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Access.Shops", t => t.ShopId)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .ForeignKey("Access.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ShopId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            AddColumn("Access.Shops", "AccountId", c => c.String(maxLength: 128));
            CreateIndex("Access.Shops", "AccountId");
            AddForeignKey("Access.Shops", "AccountId", "Access.Accounts", "AccountId");

            // Copy Shop Assignments to new Structure.
            var sql = @"DECLARE @UserId UNIQUEIDENTIFIER
                        SELECT @UserId = u.UserId
                        FROM Access.Users u
                        WHERE u.Email IN ('sandersmw@mac.com', 'sandersmw@unimatrixdesigns.com')

                        INSERT INTO Access.UserShops (UserId, ShopId, CreatedBy_Id, CreatedDt)
                        SELECT
	                        u.UserId
	                        ,u.Shop_ID
	                        ,@UserId
	                        ,GETUTCDATE()
                        FROM Access.Users u
                        LEFT JOIN Access.UserShops us
	                        ON u.UserId = us.UserId
		                        AND u.Shop_ID = us.ShopId
                        WHERE us.UserId IS NULL";
            Sql(sql);
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Shops", "AccountId", "Access.Accounts");
            DropIndex("Access.Shops", new[] { "AccountId" });
            DropColumn("Access.Shops", "AccountId");

            DropForeignKey("Access.UserShops", "UserId", "Access.Users");
            DropForeignKey("Access.UserShops", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Access.UserShops", "ShopId", "Access.Shops");
            DropForeignKey("Access.UserShops", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Access.UserGroups", "UserId", "Access.Users");
            DropForeignKey("Access.UserGroups", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Access.UserGroups", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Access.UserGroups", "GroupId", "Access.Groups");
            DropForeignKey("Access.UserAccounts", "UserId", "Access.Users");
            DropForeignKey("Access.UserAccounts", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Access.UserAccounts", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Access.UserAccounts", "AccountId", "Access.Accounts");
            DropForeignKey("Access.GroupRoles", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Access.GroupRoles", "RoleId", "Access.Roles");
            DropForeignKey("Access.GroupRoles", "GroupId", "Access.Groups");
            DropForeignKey("Access.Groups", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Access.Groups", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Access.GroupRoles", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Access.Accounts", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Access.Accounts", "CreatedBy_Id", "Access.Users");
            DropIndex("Access.UserShops", new[] { "UpdatedBy_Id" });
            DropIndex("Access.UserShops", new[] { "CreatedBy_Id" });
            DropIndex("Access.UserShops", new[] { "ShopId" });
            DropIndex("Access.UserShops", new[] { "UserId" });
            DropIndex("Access.UserGroups", new[] { "UpdatedBy_Id" });
            DropIndex("Access.UserGroups", new[] { "CreatedBy_Id" });
            DropIndex("Access.UserGroups", new[] { "GroupId" });
            DropIndex("Access.UserGroups", new[] { "UserId" });
            DropIndex("Access.UserAccounts", new[] { "UpdatedBy_Id" });
            DropIndex("Access.UserAccounts", new[] { "CreatedBy_Id" });
            DropIndex("Access.UserAccounts", new[] { "AccountId" });
            DropIndex("Access.UserAccounts", new[] { "UserId" });
            DropIndex("Access.Groups", new[] { "UpdatedBy_Id" });
            DropIndex("Access.Groups", new[] { "CreatedBy_Id" });
            DropIndex("Access.GroupRoles", new[] { "UpdatedBy_Id" });
            DropIndex("Access.GroupRoles", new[] { "CreatedBy_Id" });
            DropIndex("Access.GroupRoles", new[] { "RoleId" });
            DropIndex("Access.GroupRoles", new[] { "GroupId" });
            DropIndex("Access.Accounts", new[] { "UpdatedBy_Id" });
            DropIndex("Access.Accounts", new[] { "CreatedBy_Id" });
            DropTable("Access.UserShops");
            DropTable("Access.UserGroups");
            DropTable("Access.UserAccounts");
            DropTable("Access.Groups");
            DropTable("Access.GroupRoles");
            DropTable("Access.Accounts");
            RenameColumn(table: "Access.Roles", name: "RoleId", newName: "Id");
            RenameColumn(table: "Access.UserClaims", name: "ClaimId", newName: "Id");
            RenameColumn(table: "Access.Logins", name: "LoginId", newName: "LoginID");

            DropForeignKey("Access.Logins", "UserId", "Access.Users");
            DropIndex("Access.Logins", new[] { "UserId" });
            RenameColumn(table: "Access.Logins", name: "UserId", newName: "UserID");
            CreateIndex("Access.Logins", "UserID");
            AddForeignKey("Access.Logins", "UserID", "Access.Users", "UserId");
        }
    }
}
