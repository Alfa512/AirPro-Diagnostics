namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Repair.Invoices",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false, identity: true),
                        RepairID = c.Int(nullable: false),
                        CustomerMemo = c.String(),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        UpdatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InvoiceID)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id, cascadeDelete: false)
                .ForeignKey("Repair.Orders", t => t.RepairID, cascadeDelete: true)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .Index(t => t.RepairID)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            AddColumn("Scan.Reports", "Invoiced", c => c.Boolean(nullable: false));
            AddColumn("Scan.Reports", "InvoicedDt", c => c.DateTimeOffset(precision: 7));
            AddColumn("Scan.Reports", "InvoicedBy_Id", c => c.String(maxLength: 128));
            CreateIndex("Scan.Reports", "InvoicedBy_Id");
            AddForeignKey("Scan.Reports", "InvoicedBy_Id", "Access.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("Repair.Invoices", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Repair.Invoices", "RepairID", "Repair.Orders");
            DropForeignKey("Repair.Invoices", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Scan.Reports", "InvoicedBy_Id", "Access.Users");
            DropIndex("Scan.Reports", new[] { "InvoicedBy_Id" });
            DropIndex("Repair.Invoices", new[] { "UpdatedBy_Id" });
            DropIndex("Repair.Invoices", new[] { "CreatedBy_Id" });
            DropIndex("Repair.Invoices", new[] { "RepairID" });
            DropColumn("Scan.Reports", "InvoicedBy_Id");
            DropColumn("Scan.Reports", "InvoicedDt");
            DropColumn("Scan.Reports", "Invoiced");
            DropTable("Repair.Invoices");
        }
    }
}
