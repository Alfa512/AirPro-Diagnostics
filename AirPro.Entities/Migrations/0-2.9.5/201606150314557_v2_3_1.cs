namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Repair.Invoices", "RepairID", "Repair.Orders");
            DropIndex("Repair.Invoices", new[] { "RepairID" });
            DropPrimaryKey("Repair.Invoices");
            DropColumn("Repair.Invoices", "InvoiceID");
            RenameColumn(table: "Repair.Invoices", name: "RepairID", newName: "InvoiceID");
            AlterColumn("Repair.Invoices", "InvoiceID", c => c.Int(nullable: false));
            AddPrimaryKey("Repair.Invoices", "InvoiceID");
            CreateIndex("Repair.Invoices", "InvoiceID");
            AddForeignKey("Repair.Invoices", "InvoiceID", "Repair.Orders", "RepairOrderID");
        }
        
        public override void Down()
        {
            DropForeignKey("Repair.Invoices", "InvoiceID", "Repair.Orders");
            DropIndex("Repair.Invoices", new[] { "InvoiceID" });
            DropPrimaryKey("Repair.Invoices");
            AlterColumn("Repair.Invoices", "InvoiceID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("Repair.Invoices", "InvoiceID");
            RenameColumn(table: "Repair.Invoices", name: "InvoiceID", newName: "RepairID");
            AddColumn("Repair.Invoices", "InvoiceID", c => c.Int(nullable: false, identity: true));
            CreateIndex("Repair.Invoices", "RepairID");
            AddForeignKey("Repair.Invoices", "RepairID", "Repair.Orders", "RepairOrderID", cascadeDelete: true);
        }
    }
}
