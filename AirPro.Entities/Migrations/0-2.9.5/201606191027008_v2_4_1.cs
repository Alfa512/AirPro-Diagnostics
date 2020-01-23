namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_4_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Access.Shops", "Notes");
        }
    }
}
