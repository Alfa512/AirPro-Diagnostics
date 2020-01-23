namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDeploy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Support.ApplicationExceptions",
                c => new
                    {
                        ExceptionID = c.Int(nullable: false, identity: true),
                        ExceptionMessage = c.String(),
                        ExceptionStackTrace = c.String(),
                        ExceptionDateTime = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ExceptionID);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("Access.UserRoles", "IdentityUser_Id", "Access.Users");
            DropForeignKey("Access.UserLogins", "IdentityUser_Id", "Access.Users");
            DropForeignKey("Access.UserClaims", "IdentityUser_Id", "Access.Users");
            DropForeignKey("Access.UserRoles", "RoleId", "Access.Roles");
            DropIndex("Access.UserLogins", new[] { "IdentityUser_Id" });
            DropIndex("Access.UserClaims", new[] { "IdentityUser_Id" });
            DropIndex("Access.Users", "UserNameIndex");
            DropIndex("Access.UserRoles", new[] { "IdentityUser_Id" });
            DropIndex("Access.UserRoles", new[] { "RoleId" });
            DropIndex("Access.Roles", "RoleNameIndex");
            DropTable("Access.UserLogins");
            DropTable("Access.UserClaims");
            DropTable("Access.Users");
            DropTable("Access.UserRoles");
            DropTable("Access.Roles");
            DropTable("Support.ApplicationExceptions");
        }
    }
}
