namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD301 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetInsuranceCompanies");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_SaveInsuranceCompany");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CreateFromCCCEstimates");

            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_UpdateCCCInsuranceCompanies");

            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_RunNightlyProcess");

            CreateTable(
                "Service.CCCInsuranceCompanies",
                c => new
                    {
                        CCCInsuranceCompanyId = c.String(nullable: false, maxLength: 128),
                        CCCInsuranceCompanyName = c.String(),
                        RepairInsuranceCompanyId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.CCCInsuranceCompanyId)
                .ForeignKey("Repair.InsuranceCompanies", t => t.RepairInsuranceCompanyId)
                .Index(t => t.RepairInsuranceCompanyId);

            Sql(@"CREATE INDEX IX_ServiceCCCEstimates_InsuranceCompanyId ON Service.CCCEstimates (InsuranceCompanyId) INCLUDE (InsuranceCompanyName);");

            Sql(@"MERGE Service.CCCInsuranceCompanies AS t
                USING
                (
	                SELECT
		                i.InsuranceCompanyId [CCCInsuranceCompanyId]
		                ,n.InsuranceCompanyName [CCCInsuranceCompanyName]
		                ,ic.InsuranceCompanyId [RepairInsuranceCompanyId]
	                FROM
	                (
		                SELECT InsuranceCompanyId
		                FROM Service.CCCEstimates
		                WHERE InsuranceCompanyId IS NOT NULL
		                GROUP BY InsuranceCompanyId
	                ) i
	                OUTER APPLY
	                (
		                SELECT TOP 1 InsuranceCompanyName
		                FROM Service.CCCEstimates
		                WHERE InsuranceCompanyId = i.InsuranceCompanyId
		                ORDER BY EstimateId DESC
	                ) n
	                LEFT JOIN Repair.InsuranceCompanies ic
		                ON i.InsuranceCompanyId = ic.CCCInsuranceCompanyId
                ) AS s
                ON (t.CCCInsuranceCompanyId = s.CCCInsuranceCompanyId)
                WHEN NOT MATCHED THEN
	                INSERT (CCCInsuranceCompanyId, CCCInsuranceCompanyName, RepairInsuranceCompanyId)
	                VALUES (CCCInsuranceCompanyId, CCCInsuranceCompanyName, RepairInsuranceCompanyId)
                OUTPUT INSERTED.*;");

            DropIndex("Repair.InsuranceCompanies", new[] { "CCCInsuranceCompanyId" });
            DropColumn("Repair.InsuranceCompanies", "CCCInsuranceCompanyId");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetInsuranceCompanies");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_SaveInsuranceCompany");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CreateFromCCCEstimates");

            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_UpdateCCCInsuranceCompanies");

            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_RunNightlyProcess");

            AddColumn("Repair.InsuranceCompanies", "CCCInsuranceCompanyId", c => c.String(maxLength: 128));
            CreateIndex("Repair.InsuranceCompanies", "CCCInsuranceCompanyId");

            Sql(@"MERGE Repair.InsuranceCompanies AS t
                USING
                (
                    SELECT
	                    r.RepairInsuranceCompanyId
	                    ,i.CCCInsuranceCompanyId
                    FROM
                    (
	                    SELECT ic.RepairInsuranceCompanyId
	                    FROM Service.CCCInsuranceCompanies ic
	                    WHERE ic.RepairInsuranceCompanyId IS NOT NULL
	                    GROUP BY ic.RepairInsuranceCompanyId
                    ) r
                    CROSS APPLY
                    (
	                    SELECT TOP 1 CCCInsuranceCompanyId
	                    FROM Service.CCCInsuranceCompanies
	                    WHERE RepairInsuranceCompanyId = r.RepairInsuranceCompanyId
                    ) i
                ) AS s
                ON (s.RepairInsuranceCompanyId = t.InsuranceCompanyId)
                WHEN MATCHED THEN
	                UPDATE SET CCCInsuranceCompanyId = s.CCCInsuranceCompanyId
                OUTPUT INSERTED.*;");

            Sql(@"DROP INDEX IF EXISTS IX_ServiceCCCEstimates_InsuranceCompanyId ON Service.CCCEstimates;");

            DropForeignKey("Service.CCCInsuranceCompanies", "RepairInsuranceCompanyId", "Repair.InsuranceCompanies");
            DropIndex("Service.CCCInsuranceCompanies", new[] { "RepairInsuranceCompanyId" });
            DropTable("Service.CCCInsuranceCompanies");
        }
    }
}
