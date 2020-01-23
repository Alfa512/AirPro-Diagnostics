using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1760 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Service.MitchellRegistrations",
                c => new
                    {
                        MitchellRegistrationId = c.Int(nullable: false, identity: true),
                        MitchellAccountId = c.String(),
                        CallbackUrl = c.String(),
                        UserEmail = c.String(),
                        RequestDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        CallbackPerformedDt = c.DateTimeOffset(nullable: true, precision: 7)
                })
                .PrimaryKey(t => t.MitchellRegistrationId);

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_GetMitchellRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_SaveMitchellRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_UpdateMitchellRegistration");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_GetMitchellRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_SaveMitchellRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_UpdateMitchellRegistration");

            DropTable("Service.MitchellRegistrations");
        }
    }
}
