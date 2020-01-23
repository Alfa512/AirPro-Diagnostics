namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Repair.Orders", "Odometer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Repair.Orders", "Odometer", c => c.String());
        }
    }
}
