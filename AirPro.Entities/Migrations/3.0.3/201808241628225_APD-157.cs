using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD157 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Repair.Orders", "AirBagsVisualDeployments", c => c.String());

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetTechnicianRequest");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetTechnicianRequest");

            DropColumn("Repair.Orders", "AirBagsVisualDeployments");
        }
    }
}
