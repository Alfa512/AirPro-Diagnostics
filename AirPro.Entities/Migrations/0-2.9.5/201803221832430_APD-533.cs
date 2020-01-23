namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD533 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "AverageVehiclesPerMonth", c => c.Int(nullable: true));
        }

        public override void Down()
        {
            DropColumn("Access.Shops", "AverageVehiclesPerMonth");
        }
    }
}
