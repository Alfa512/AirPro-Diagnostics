using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.RegistrationShops", "AllowSelfScanAssessment", c => c.Boolean(nullable: false));

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveRegistration"); 
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_CompleteRegistration");
        }
        
        public override void Down()
        {
            DropColumn("Access.RegistrationShops", "AllowSelfScanAssessment");

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_CompleteRegistration");
        }
    }
}
