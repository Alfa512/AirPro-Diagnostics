namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2104 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Repair.Feedback", "CreatedByUserGuid", c => c.Guid(nullable: false));
            AddColumn("Repair.Feedback", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Repair.Feedback", "UpdatedByUserGuid", c => c.Guid());
            AddColumn("Repair.Feedback", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            CreateIndex("Repair.Feedback", "CreatedByUserGuid");
            CreateIndex("Repair.Feedback", "UpdatedByUserGuid");
            AddForeignKey("Repair.Feedback", "CreatedByUserGuid", "Access.Users", "UserGuid", cascadeDelete: true);
            AddForeignKey("Repair.Feedback", "UpdatedByUserGuid", "Access.Users", "UserGuid");
        }
        
        public override void Down()
        {
            DropForeignKey("Repair.Feedback", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Repair.Feedback", "CreatedByUserGuid", "Access.Users");
            DropIndex("Repair.Feedback", new[] { "UpdatedByUserGuid" });
            DropIndex("Repair.Feedback", new[] { "CreatedByUserGuid" });
            DropColumn("Repair.Feedback", "UpdatedDt");
            DropColumn("Repair.Feedback", "UpdatedByUserGuid");
            DropColumn("Repair.Feedback", "CreatedDt");
            DropColumn("Repair.Feedback", "CreatedByUserGuid");
        }
    }
}
