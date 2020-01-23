namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_8 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("Support.NotificationTemplates", "Body", "EmailBody");
            //AddColumn("Support.NotificationTemplates", "EmailBody", c => c.String());
            //DropColumn("Support.NotificationTemplates", "Body");

            AddColumn("Support.NotificationTemplates", "TextMessage", c => c.String());
            DropColumn("Support.NotificationTemplates", "Type");
        }
        
        public override void Down()
        {
            RenameColumn("Support.NotificationTemplates", "EmailBody", "Body");
            //AddColumn("Support.NotificationTemplates", "Body", c => c.String());
            //DropColumn("Support.NotificationTemplates", "EmailBody");

            AddColumn("Support.NotificationTemplates", "Type", c => c.Int(nullable: false));
            DropColumn("Support.NotificationTemplates", "TextMessage");
        }
    }
}
