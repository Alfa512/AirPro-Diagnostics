namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v300 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Repair.Invoices", "InvoicedByUserGuid", "Access.Users");
            DropIndex("Repair.Invoices", new[] { "InvoicedByUserGuid" });
            AddColumn("Access.Users", "ConnectionId", c => c.String());
            AddColumn("Access.Users", "ConnectionLastDt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("Repair.Invoices", "InvoicedByUserGuid", c => c.Guid());
            CreateIndex("Repair.Invoices", "InvoicedByUserGuid");
            AddForeignKey("Repair.Invoices", "InvoicedByUserGuid", "Access.Users", "UserGuid");
        }
        
        public override void Down()
        {
            DropForeignKey("Repair.Invoices", "InvoicedByUserGuid", "Access.Users");
            DropIndex("Repair.Invoices", new[] { "InvoicedByUserGuid" });
            AlterColumn("Repair.Invoices", "InvoicedByUserGuid", c => c.Guid(nullable: false));
            DropColumn("Access.Users", "ConnectionLastDt");
            DropColumn("Access.Users", "ConnectionId");
            CreateIndex("Repair.Invoices", "InvoicedByUserGuid");
            AddForeignKey("Repair.Invoices", "InvoicedByUserGuid", "Access.Users", "UserGuid", cascadeDelete: true);
        }
    }
}
