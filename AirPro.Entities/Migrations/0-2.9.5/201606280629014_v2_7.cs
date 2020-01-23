namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Access.Logins",
                c => new
                    {
                        LoginID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        LoginName = c.String(),
                        UserAgent = c.String(),
                        UserHostAddress = c.String(),
                        UserHostName = c.String(),
                        AccountLockedOut = c.Boolean(nullable: false),
                        TwoFactorChallenge = c.Boolean(nullable: false),
                        TwoFactorVerified = c.Boolean(nullable: false),
                        LoginAttemptDt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.LoginID)
                .ForeignKey("Access.Users", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Logins", "UserID", "Access.Users");
            DropIndex("Access.Logins", new[] { "UserID" });
            DropTable("Access.Logins");
        }
    }
}
