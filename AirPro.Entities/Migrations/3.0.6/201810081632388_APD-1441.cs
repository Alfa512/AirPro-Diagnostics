using UniMatrix.Common.Enumerations;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Extensions;

    public partial class APD1441 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatus");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatusByDate");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatus");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatusByDate");
        }
    }
}
