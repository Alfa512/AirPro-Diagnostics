using UniMatrix.Common.Enumerations;

namespace AirPro.Entities.Migrations
{
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Extensions;

    public partial class APD1159 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetAgingRepairsByUser");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetAgingRepairsByUser");
        }
    }
}
