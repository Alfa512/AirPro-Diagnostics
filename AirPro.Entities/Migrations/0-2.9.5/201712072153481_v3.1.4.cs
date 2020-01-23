namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v314 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Scan.ReportWorkTypes",
                c => new
                    {
                        ReportId = c.Int(nullable: false),
                        WorkTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReportId, t.WorkTypeId })
                .ForeignKey("Scan.Reports", t => t.ReportId, cascadeDelete: true)
                .ForeignKey("Scan.WorkTypes", t => t.WorkTypeId, cascadeDelete: true)
                .Index(t => t.ReportId)
                .Index(t => t.WorkTypeId);
            
            CreateTable(
                "Scan.WorkTypes",
                c => new
                    {
                        WorkTypeId = c.Int(nullable: false, identity: true),
                        WorkTypeName = c.String(),
                        WorkTypeSortOrder = c.Int(),
                        WorkTypeGroupId = c.Int(nullable: false),
                        WorkTypeActiveInd = c.Boolean(nullable: false),
                        WorkTypeDescription = c.String(),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.WorkTypeId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .ForeignKey("Scan.WorkTypeGroups", t => t.WorkTypeGroupId, cascadeDelete: true)
                .Index(t => t.WorkTypeGroupId)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Scan.WorkTypeGroups",
                c => new
                    {
                        WorkTypeGroupId = c.Int(nullable: false, identity: true),
                        WorkTypeGroupName = c.String(),
                        WorkTypeGroupSortOrder = c.Int(),
                        WorkTypeGroupActiveInd = c.Boolean(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.WorkTypeGroupId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Scan.WorkTypeRequestTypes",
                c => new
                    {
                        WorkTypeId = c.Int(nullable: false),
                        RequestTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkTypeId, t.RequestTypeId })
                .ForeignKey("Scan.RequestTypes", t => t.RequestTypeId, cascadeDelete: true)
                .ForeignKey("Scan.WorkTypes", t => t.WorkTypeId, cascadeDelete: true)
                .Index(t => t.WorkTypeId)
                .Index(t => t.RequestTypeId);

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Common' AND ROUTINE_NAME = 'udf_IdListToTable') DROP FUNCTION [Common].[udf_IdListToTable];");

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetWorkTypeGroups') DROP PROCEDURE [Scan].[usp_GetWorkTypeGroups];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetWorkTypes') DROP PROCEDURE [Scan].[usp_GetWorkTypes];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_SaveWorkType') DROP PROCEDURE [Scan].[usp_SaveWorkType];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_SaveWorkTypeGroup') DROP PROCEDURE [Scan].[usp_SaveWorkTypeGroup];");

        }

        public override void Down()
        {
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Common' AND ROUTINE_NAME = 'udf_IdListToTable') DROP FUNCTION [Common].[udf_IdListToTable];");

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetWorkTypeGroups') DROP PROCEDURE [Scan].[usp_GetWorkTypeGroups];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetWorkTypes') DROP PROCEDURE [Scan].[usp_GetWorkTypes];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_SaveWorkType') DROP PROCEDURE [Scan].[usp_SaveWorkType];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_SaveWorkTypeGroup') DROP PROCEDURE [Scan].[usp_SaveWorkTypeGroup];");

            DropForeignKey("Scan.ReportWorkTypes", "WorkTypeId", "Scan.WorkTypes");
            DropForeignKey("Scan.WorkTypeRequestTypes", "WorkTypeId", "Scan.WorkTypes");
            DropForeignKey("Scan.WorkTypeRequestTypes", "RequestTypeId", "Scan.RequestTypes");
            DropForeignKey("Scan.WorkTypes", "WorkTypeGroupId", "Scan.WorkTypeGroups");
            DropForeignKey("Scan.WorkTypeGroups", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.WorkTypeGroups", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.WorkTypes", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.WorkTypes", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.ReportWorkTypes", "ReportId", "Scan.Reports");
            DropIndex("Scan.WorkTypeRequestTypes", new[] { "RequestTypeId" });
            DropIndex("Scan.WorkTypeRequestTypes", new[] { "WorkTypeId" });
            DropIndex("Scan.WorkTypeGroups", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.WorkTypeGroups", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.WorkTypes", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.WorkTypes", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.WorkTypes", new[] { "WorkTypeGroupId" });
            DropIndex("Scan.ReportWorkTypes", new[] { "WorkTypeId" });
            DropIndex("Scan.ReportWorkTypes", new[] { "ReportId" });
            DropTable("Scan.WorkTypeRequestTypes");
            DropTable("Scan.WorkTypeGroups");
            DropTable("Scan.WorkTypes");
            DropTable("Scan.ReportWorkTypes");
        }
    }
}
