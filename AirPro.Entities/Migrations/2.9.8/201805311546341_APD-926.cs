namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD926 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.RequestTypes", "InvoiceMemo", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("Scan.RequestTypes", "InvoiceMemo");
        }
    }
}
