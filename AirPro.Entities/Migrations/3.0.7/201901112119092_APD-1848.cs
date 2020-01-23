namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1848 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestById");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestById");
        }
    }
}
