namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Support.NotificationTemplates",
                c => new
                    {
                        NotificationTemplateID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Options = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        UpdatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NotificationTemplateID)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id, cascadeDelete: true)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Support.NotificationTemplates", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Support.NotificationTemplates", "CreatedBy_Id", "Access.Users");
            DropIndex("Support.NotificationTemplates", new[] { "UpdatedBy_Id" });
            DropIndex("Support.NotificationTemplates", new[] { "CreatedBy_Id" });
            DropTable("Support.NotificationTemplates");
        }
    }
}
