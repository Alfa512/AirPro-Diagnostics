using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD473 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Repair.InsuranceCompanies", "DisabledInd", c => c.Boolean(nullable: false));
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_SaveInsuranceCompany");
        }
        
        public override void Down()
        {
            DropColumn("Repair.InsuranceCompanies", "DisabledInd");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_SaveInsuranceCompany");
        }
    }
}
