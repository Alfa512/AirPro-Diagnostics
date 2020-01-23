namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Repair.Invoices", "CustomerVisible", c => c.Boolean(nullable: false));
            AddColumn("Scan.Reports", "InvoiceAmount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Scan.Reports", "InvoiceAmount");
            DropColumn("Repair.Invoices", "CustomerVisible");
        }
    }
}
