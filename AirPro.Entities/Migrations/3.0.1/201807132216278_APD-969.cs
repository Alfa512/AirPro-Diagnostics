using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD969 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CreateFromCCCEstimates");
            this.DropObjectIfExists(DropObjectType.Function, "Common", "udf_XmlToTable");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CreateFromCCCEstimates");
            this.DropObjectIfExists(DropObjectType.Function, "Common", "udf_XmlToTable");
        }
    }
}
