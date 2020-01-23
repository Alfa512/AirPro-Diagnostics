namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_6 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Repair.Photos", name: "Repair_RepairOrderID", newName: "RepairOrderEntityModel_RepairOrderID");
            RenameIndex(table: "Repair.Photos", name: "IX_Repair_RepairOrderID", newName: "IX_RepairOrderEntityModel_RepairOrderID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Repair.Photos", name: "IX_RepairOrderEntityModel_RepairOrderID", newName: "IX_Repair_RepairOrderID");
            RenameColumn(table: "Repair.Photos", name: "RepairOrderEntityModel_RepairOrderID", newName: "Repair_RepairOrderID");
        }
    }
}
