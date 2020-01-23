using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1929 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.AccountsArchive", "EmployeeGuid", c => c.Guid());
            AddColumn("Access.Accounts", "EmployeeGuid", c => c.Guid());
            AddColumn("Access.Users", "EmployeeInd", c => c.Boolean(nullable: false));
            AddColumn("Access.Shops", "EmployeeGuid", c => c.Guid());
            AddColumn("Access.ShopsArchive", "EmployeeGuid", c => c.Guid());
            AddColumn("Access.UsersArchive", "EmployeeInd", c => c.Boolean(nullable: false));
            CreateIndex("Access.Accounts", "EmployeeGuid");
            CreateIndex("Access.Shops", "EmployeeGuid");
            AddForeignKey("Access.Accounts", "EmployeeGuid", "Access.Users", "UserGuid");
            AddForeignKey("Access.Shops", "EmployeeGuid", "Access.Users", "UserGuid");

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetEmployeeDisplayList");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessAccountsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUsersArchive");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetAccountShopRepsReport");

            Sql(@"MERGE Reporting.ReportTemplates AS t
                USING
                (
	                SELECT
		                'Account Shop Reps Report' [TemplateName]
		                ,'Provides Account Shop Reps List.' [TemplateDescription]
		                ,(SELECT MAX(TemplateSortOrder) + 1 FROM Reporting.ReportTemplates) [TemplateSortOrder]
		                ,'Reporting.usp_GetAccountShopRepsReport' [ProcedureName]
                ) AS s
                ON (s.TemplateName = t.TemplateName)
                WHEN NOT MATCHED THEN
	                INSERT (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, AccessRoles, ActiveInd)
	                VALUES (TemplateName, TemplateDescription, TemplateSortOrder, ProcedureName, 'SystemReportingView', 1)
                OUTPUT INSERTED.*;");
        }

        public override void Down()
        {
            DropForeignKey("Access.Shops", "EmployeeGuid", "Access.Users");
            DropForeignKey("Access.Accounts", "EmployeeGuid", "Access.Users");
            DropIndex("Access.Shops", new[] { "EmployeeGuid" });
            DropIndex("Access.Accounts", new[] { "EmployeeGuid" });
            DropColumn("Access.UsersArchive", "EmployeeInd");
            DropColumn("Access.ShopsArchive", "EmployeeGuid");
            DropColumn("Access.Shops", "EmployeeGuid");
            DropColumn("Access.Users", "EmployeeInd");
            DropColumn("Access.Accounts", "EmployeeGuid");
            DropColumn("Access.AccountsArchive", "EmployeeGuid");

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetEmployeeDisplayList");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessAccountsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUsersArchive");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");

            Sql(@"DELETE FROM Reporting.ReportTemplates WHERE TemplateName = 'Account Shop Reps Report';");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetAccountShopRepsReport");
        }
    }
}
