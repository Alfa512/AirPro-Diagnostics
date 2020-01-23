namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD187 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Reports", "CanceledInd", c => c.Boolean(nullable: false));
            AddColumn("Scan.Reports", "CancellationNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Scan.Reports", "CancellationNotes");
            DropColumn("Scan.Reports", "CanceledInd");
        }
    }
}
