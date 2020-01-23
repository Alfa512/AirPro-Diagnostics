using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2282 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_SaveInsuranceCompany");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_SaveInsuranceCompany");
        }
    }
}
