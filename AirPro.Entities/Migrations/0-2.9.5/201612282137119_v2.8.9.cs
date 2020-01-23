using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v289 : DbMigration
    {
        public override void Up()
        {
            // Scan Reports.
            DropForeignKey("Scan.Reports", "CompletedByID", "Access.Users");
            DropForeignKey("Scan.Reports", "InvoicedByID", "Access.Users");
            DropForeignKey("Scan.Reports", "ResponsibleTechnicianID", "Access.Users");

            DropIndex("Scan.Reports", new[] { "CompletedByID" });
            DropIndex("Scan.Reports", new[] { "InvoicedByID" });
            DropIndex("Scan.Reports", new[] { "ResponsibleTechnicianID" });

            RenameColumn("Scan.Reports", "CompletedByID", "CompletedByUserId");
            RenameColumn("Scan.Reports", "InvoicedByID", "InvoicedByUserId");
            RenameColumn("Scan.Reports", "ResponsibleTechnicianID", "ResponsibleTechnicianUserId");
            RenameColumn("Scan.ReportsArchive", "CompletedByID", "CompletedByUserId");
            RenameColumn("Scan.ReportsArchive", "InvoicedByID", "InvoicedByUserId");
            RenameColumn("Scan.ReportsArchive", "ResponsibleTechnicianID", "ResponsibleTechnicianUserId");

            CreateIndex("Scan.Reports", new [] { "CompletedByUserId" });
            CreateIndex("Scan.Reports", new[] { "InvoicedByUserId" });
            CreateIndex("Scan.Reports", new[] { "ResponsibleTechnicianUserId" });

            AddForeignKey("Scan.Reports", "CompletedByUserId", "Access.Users", "UserId");
            AddForeignKey("Scan.Reports", "InvoicedByUserId", "Access.Users", "UserId");
            AddForeignKey("Scan.Reports", "ResponsibleTechnicianUserId", "Access.Users", "UserId");

            // Scan Requests Types.
            DropForeignKey("Scan.Requests", "ReportID", "Scan.Reports");
            DropForeignKey("Scan.Requests", "TypeOfScan", "Scan.RequestTypes");
            DropForeignKey("Scan.RequestTypes", "NextRequestTypeID", "Scan.RequestTypes");

            DropIndex("Scan.RequestTypes", new[] { "RequestTypeID" });
            DropIndex("Scan.RequestTypes", new[] { "NextRequestTypeID" });
            DropIndex("Scan.Requests", new[] { "TypeOfScan" });
            DropIndex("Scan.Requests", new[] { "ReportID" });
            DropIndex("Scan.Requests", new[] { "RequestID" });

            DropPrimaryKey("Scan.RequestTypes");

            RenameColumn("Scan.RequestTypes", "RequestTypeID", "RequestTypeId");
            RenameColumn("Scan.RequestTypes", "NextRequestTypeID", "NextRequestTypeId");
            RenameColumn("Scan.Requests", "TypeOfScan", "RequestTypeId");
            RenameColumn("Scan.Requests", "ReportID", "ReportId");
            RenameColumn("Scan.Requests", "RequestID", "RequestId");

            AddPrimaryKey("Scan.RequestTypes", "RequestTypeId");

            CreateIndex("Scan.RequestTypes", new[] { "RequestTypeId" });
            CreateIndex("Scan.RequestTypes", new[] { "NextRequestTypeId" });
            CreateIndex("Scan.Requests", new[] { "RequestTypeId" });
            CreateIndex("Scan.Requests", new[] { "ReportId" });
            CreateIndex("Scan.Requests", new[] { "RequestId" });

            AddForeignKey("Scan.RequestTypes", "NextRequestTypeId", "Scan.RequestTypes", "RequestTypeId");
            AddForeignKey("Scan.Requests", "ReportId", "Scan.Reports");
            AddForeignKey("Scan.Requests", "RequestTypeId", "Scan.RequestTypes");

            RenameColumn(table: "Scan.Requests", name: "Repair_RepairOrderID", newName: "RepairId");
            RenameIndex(table: "Scan.Requests", name: "IX_Repair_RepairOrderID", newName: "IX_RepairId");

            // Warning Indicators.
            DropForeignKey("Scan.RequestWarningIndicators", "RequestID", "Scan.Requests");
            DropForeignKey("Scan.RequestWarningIndicators", "WarningIndicatorID", "Scan.WarningIndicators");

            DropIndex("Scan.RequestWarningIndicators", new[] { "RequestID" });
            DropIndex("Scan.RequestWarningIndicators", new[] { "WarningIndicatorID" });

            DropPrimaryKey("Scan.WarningIndicators");
            DropPrimaryKey("Scan.RequestWarningIndicators");

            DropColumn("Scan.RequestWarningIndicators", "RequestWarningIndicatorID");
            RenameColumn("Scan.RequestWarningIndicators", "RequestID", "RequestId");
            RenameColumn("Scan.RequestWarningIndicators", "WarningIndicatorID", "WarningIndicatorId");
            RenameColumn("Scan.WarningIndicators", "WarningIndicatorID", "WarningIndicatorId");

            AddPrimaryKey("Scan.RequestWarningIndicators", new[] { "RequestId", "WarningIndicatorId" });
            AddPrimaryKey("Scan.WarningIndicators", new[] { "WarningIndicatorId" });

            CreateIndex("Scan.RequestWarningIndicators", new[] { "RequestId" });
            CreateIndex("Scan.RequestWarningIndicators", new[] { "WarningIndicatorId" });

            AddForeignKey("Scan.RequestWarningIndicators", "RequestId", "Scan.Requests");
            AddForeignKey("Scan.RequestWarningIndicators", "WarningIndicatorId", "Scan.WarningIndicators");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612282137119_v2.8.9_Deploy.sql");

        }

        public override void Down()
        {

            // Scan Reports.
            DropForeignKey("Scan.Reports", "CompletedByUserId", "Access.Users");
            DropForeignKey("Scan.Reports", "InvoicedByUserId", "Access.Users");
            DropForeignKey("Scan.Reports", "ResponsibleTechnicianUserId", "Access.Users");

            DropIndex("Scan.Reports", new[] { "CompletedByUserId" });
            DropIndex("Scan.Reports", new[] { "InvoicedByUserId" });
            DropIndex("Scan.Reports", new[] { "ResponsibleTechnicianUserId" });

            RenameColumn("Scan.Reports", "CompletedByUserId", "CompletedByID");
            RenameColumn("Scan.Reports", "InvoicedByUserId", "InvoicedByID");
            RenameColumn("Scan.Reports", "ResponsibleTechnicianUserId", "ResponsibleTechnicianID");

            RenameColumn("Scan.ReportsArchive", "CompletedByUserId", "CompletedByID");
            RenameColumn("Scan.ReportsArchive", "InvoicedByUserId", "InvoicedByID");
            RenameColumn("Scan.ReportsArchive", "ResponsibleTechnicianUserId", "ResponsibleTechnicianID");

            CreateIndex("Scan.Reports", new[] { "CompletedByID" });
            CreateIndex("Scan.Reports", new[] { "InvoicedByID" });
            CreateIndex("Scan.Reports", new[] { "ResponsibleTechnicianID" });

            AddForeignKey("Scan.Reports", "CompletedByID", "Access.Users", "UserId");
            AddForeignKey("Scan.Reports", "InvoicedByID", "Access.Users", "UserId");
            AddForeignKey("Scan.Reports", "ResponsibleTechnicianID", "Access.Users", "UserId");

            // Scan Requests Types.
            DropForeignKey("Scan.Requests", "ReportId", "Scan.Reports");
            DropForeignKey("Scan.Requests", "RequestTypeId", "Scan.RequestTypes");
            DropForeignKey("Scan.RequestTypes", "NextRequestTypeId", "Scan.RequestTypes");

            DropIndex("Scan.Requests", new[] { "ReportId" });
            DropIndex("Scan.Requests", new[] { "RequestTypeId" });
            DropIndex("Scan.RequestTypes", new[] { "RequestTypeId" });
            DropIndex("Scan.RequestTypes", new[] { "NextRequestTypeId" });

            DropPrimaryKey("Scan.RequestTypes");

            RenameColumn("Scan.Requests", "RequestTypeId", "TypeOfScan");
            RenameColumn("Scan.Requests", "ReportId", "ReportID");
            RenameColumn("Scan.RequestTypes", "RequestTypeId", "RequestTypeID");
            RenameColumn("Scan.RequestTypes", "NextRequestTypeId", "NextRequestTypeID");
            RenameColumn("Scan.Requests", "RequestId", "RequestID");

            AddPrimaryKey("Scan.RequestTypes", "RequestTypeID");

            CreateIndex("Scan.Requests", new[] { "TypeOfScan" });
            CreateIndex("Scan.Requests", new[] { "ReportID" });
            CreateIndex("Scan.RequestTypes", new[] { "RequestTypeID" });
            CreateIndex("Scan.RequestTypes", new[] { "NextRequestTypeID" });

            AddForeignKey("Scan.RequestTypes", "NextRequestTypeID", "Scan.RequestTypes", "RequestTypeId");
            AddForeignKey("Scan.Requests", "ReportID", "Scan.Reports", "ReportId");
            AddForeignKey("Scan.Requests", "TypeOfScan", "Scan.RequestTypes", "RequestTypeId");

            RenameColumn("Scan.Requests", "RepairId", "Repair_RepairOrderID");
            RenameIndex("Scan.Requests", "IX_RepairId", "IX_Repair_RepairOrderID");

            // Warning Indicators.
            DropForeignKey("Scan.RequestWarningIndicators", "RequestId", "Scan.Requests");
            DropForeignKey("Scan.RequestWarningIndicators", "WarningIndicatorId", "Scan.WarningIndicators");

            DropIndex("Scan.RequestWarningIndicators", new[] { "RequestId" });
            DropIndex("Scan.RequestWarningIndicators", new[] { "WarningIndicatorId" });

            DropPrimaryKey("Scan.WarningIndicators");
            DropPrimaryKey("Scan.RequestWarningIndicators");

            RenameColumn("Scan.WarningIndicators", "WarningIndicatorId", "WarningIndicatorID");
            RenameColumn("Scan.RequestWarningIndicators", "RequestId", "RequestID");
            RenameColumn("Scan.RequestWarningIndicators", "WarningIndicatorId", "WarningIndicatorID");
            AddColumn("Scan.RequestWarningIndicators", "RequestWarningIndicatorID", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("Scan.WarningIndicators", new[] { "WarningIndicatorID" });
            AddPrimaryKey("Scan.RequestWarningIndicators", new[] { "RequestWarningIndicatorID" });

            CreateIndex("Scan.RequestWarningIndicators", new[] { "RequestID" });
            CreateIndex("Scan.RequestWarningIndicators", new[] { "WarningIndicatorID" });

            AddForeignKey("Scan.RequestWarningIndicators", "RequestID", "Scan.Requests");
            AddForeignKey("Scan.RequestWarningIndicators", "WarningIndicatorID", "Scan.WarningIndicators");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612282137119_v2.8.9_Rollback.sql");

        }
    }
}
