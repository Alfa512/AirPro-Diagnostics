namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD325 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Repair.Feedback",
                c => new
                    {
                        RepairId = c.Int(nullable: false),
                        ResponseTimeRate = c.Int(nullable: false),
                        RequestTimeRate = c.Int(nullable: false),
                        TechnicianKnowledgeRate = c.Int(nullable: false),
                        ReportCompletionRate = c.Int(nullable: false),
                        ConcernsAddressedRate = c.Int(nullable: false),
                        TechnicianCommunicationRate = c.Int(nullable: false),
                        AdditionalFeedback = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.RepairId)
                .ForeignKey("Repair.Orders", t => t.RepairId)
                .Index(t => t.RepairId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Repair.Feedback", "RepairId", "Repair.Orders");
            DropIndex("Repair.Feedback", new[] { "RepairId" });
            DropTable("Repair.Feedback");
        }
    }
}
