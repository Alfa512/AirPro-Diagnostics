namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1980 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Scan.RequestTypes", "InvoiceMemo", c => c.String(maxLength: 800));
        }
        
        public override void Down()
        {
            AlterColumn("Scan.RequestTypes", "InvoiceMemo", c => c.String(maxLength: 200));
        }
    }
}
