namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v321 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Requests", "Contact", builder => builder.String(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("Scan.Requests", "Contact");
        }
    }
}
