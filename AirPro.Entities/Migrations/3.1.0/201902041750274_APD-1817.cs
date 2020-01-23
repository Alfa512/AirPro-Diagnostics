using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1817 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Trigger, "Scan", "trgScanReportsArchive");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            DropForeignKey("Scan.Reports", "DiagnosticResultId", "Diagnostic.Results");
            DropIndex("Scan.Reports", new[] { "DiagnosticResultId" });
            CreateTable(
                "Scan.ReportDiagnosticResults",
                c => new
                    {
                        ReportId = c.Int(nullable: false),
                        DiagnosticResultId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReportId, t.DiagnosticResultId })
                .ForeignKey("Diagnostic.Results", t => t.DiagnosticResultId, cascadeDelete: true)
                .ForeignKey("Scan.Reports", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId)
                .Index(t => t.DiagnosticResultId);
            
            Sql(@"MERGE Scan.ReportDiagnosticResults AS t
                USING
                (
	                SELECT
		                r.ReportId
		                ,r.DiagnosticResultId
	                FROM Scan.Reports r
	                WHERE r.DiagnosticResultId IS NOT NULL
                ) AS s
                ON (t.ReportId = s.ReportId AND t.DiagnosticResultId = s.DiagnosticResultId)
                WHEN NOT MATCHED BY TARGET THEN
	                INSERT (ReportId, DiagnosticResultId)
	                VALUES (ReportId, DiagnosticResultId)
                OUTPUT INSERTED.*;");

            DropColumn("Scan.Reports", "DiagnosticResultId");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Trigger, "Scan", "trgScanReportsArchive");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            AddColumn("Scan.Reports", "DiagnosticResultId", c => c.Int());

            Sql(@"UPDATE r
	                SET r.DiagnosticResultId = rdr.DiagnosticResultId
                FROM Scan.Reports r
                INNER JOIN Scan.ReportDiagnosticResults rdr
	                ON r.ReportId = rdr.ReportId;");

            DropForeignKey("Scan.ReportDiagnosticResults", "ReportId", "Scan.Reports");
            DropForeignKey("Scan.ReportDiagnosticResults", "DiagnosticResultId", "Diagnostic.Results");
            DropIndex("Scan.ReportDiagnosticResults", new[] { "DiagnosticResultId" });
            DropIndex("Scan.ReportDiagnosticResults", new[] { "ReportId" });
            DropTable("Scan.ReportDiagnosticResults");
            CreateIndex("Scan.Reports", "DiagnosticResultId");
            AddForeignKey("Scan.Reports", "DiagnosticResultId", "Diagnostic.Results", "ResultId");
        }
    }
}
