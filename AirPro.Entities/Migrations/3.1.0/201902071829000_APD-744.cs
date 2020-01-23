namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD744 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.RequestTypes", "Instructions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Scan.RequestTypes", "Instructions");
        }
    }
}
