namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class v1_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Access.UserClaims", "IdentityUser_Id", "Access.Users");
            DropForeignKey("Access.UserLogins", "IdentityUser_Id", "Access.Users");
            DropForeignKey("Access.UserRoles", "IdentityUser_Id", "Access.Users");

            DropIndex("Access.UserRoles", new[] { "IdentityUser_Id" });
            DropIndex("Access.UserClaims", new[] { "IdentityUser_Id" });
            DropIndex("Access.UserLogins", new[] { "IdentityUser_Id" });

            DropColumn("Access.UserRoles", "IdentityUser_Id");
            DropColumn("Access.UserClaims", "IdentityUser_Id");
            DropColumn("Access.UserLogins", "IdentityUser_Id");

            DropPrimaryKey("Access.UserRoles");
            DropPrimaryKey("Access.UserLogins");

            AlterColumn("Access.UserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Access.UserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Access.UserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));

            AddPrimaryKey("Access.UserRoles", new[] { "UserId", "RoleId" });
            AddPrimaryKey("Access.UserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });

            CreateIndex("Access.UserRoles", "UserId");
            CreateIndex("Access.UserClaims", "UserId");
            CreateIndex("Access.UserLogins", "UserId");

            AddForeignKey("Access.UserClaims", "UserId", "Access.Users", "UserId", cascadeDelete: true);
            AddForeignKey("Access.UserLogins", "UserId", "Access.Users", "UserId", cascadeDelete: true);
            AddForeignKey("Access.UserRoles", "UserId", "Access.Users", "UserId", cascadeDelete: true);

            DropColumn("Access.Users", "Discriminator");
        }

        public override void Down()
        {
            DropForeignKey("Access.UserRoles", "UserId", "Access.Users");
            DropForeignKey("Access.UserRoles", "RoleId", "Access.Roles");
            DropForeignKey("Access.UserLogins", "UserId", "Access.Users");
            DropForeignKey("Access.UserClaims", "UserId", "Access.Users");

            DropForeignKey("Access.Users", "Shop_ID", "Access.Shops");

            DropForeignKey("Access.Shops", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Scan.Requests", "CreatedBy_Id", "Access.Users");

            DropPrimaryKey("Access.UserRoles");
            DropPrimaryKey("Access.UserLogins");
            DropPrimaryKey("Access.UserClaims");
            DropPrimaryKey("Access.Roles");
            DropPrimaryKey("Access.Users");

            RenameTable("Access.UserRoles", "Access.UserRoles_v12Rollback");
            RenameTable("Access.UserLogins", "Access.UserLogins_v12Rollback");
            RenameTable("Access.UserClaims", "Access.UserClaims_v12Rollback");
            RenameTable("Access.Roles", "Access.Roles_v12Rollback");
            RenameTable("Access.Users", "Access.Users_v12Rollback");

            CreateTable(
                "Access.Roles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "Access.UserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                    IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Access.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Access.Users", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);

            CreateTable(
                "Access.Users",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                    Discriminator = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "Access.UserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                    IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Access.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);

            CreateTable(
                "Access.UserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                    IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Access.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);

            AddColumn("Access.Users", "FirstName", c => c.String());
            AddColumn("Access.Users", "LastName", c => c.String());
            AddColumn("Access.Users", "JobTitle", c => c.String());
            AddColumn("Access.Users", "Shop_ID", c => c.Int());
            CreateIndex("Access.Users", "Shop_ID");
            AddForeignKey("Access.Users", "Shop_ID", "Access.Shops", "ID");
            AddForeignKey("Scan.Requests", "CreatedBy_Id", "Access.Users", "UserID");
        }
    }
}
