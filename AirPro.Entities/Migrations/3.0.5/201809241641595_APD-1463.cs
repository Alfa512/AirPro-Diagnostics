using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1463 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestGridByUser");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestGridByUser");
        }
    }
}
