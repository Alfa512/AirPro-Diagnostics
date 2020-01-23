namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v296 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Groups", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Access.Groups", "Description");
        }
    }
}
