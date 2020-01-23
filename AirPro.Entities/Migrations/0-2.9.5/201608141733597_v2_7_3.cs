namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Support.NotificationLog",
                c => new
                    {
                        NotificationLogID = c.Int(nullable: false, identity: true),
                        Destination = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        StatusMessage = c.String(),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.NotificationLogID);
            
        }
        
        public override void Down()
        {
            DropTable("Support.NotificationLog");
        }
    }
}
