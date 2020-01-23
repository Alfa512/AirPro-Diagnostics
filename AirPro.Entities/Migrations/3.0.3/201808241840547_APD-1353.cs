using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class APD1353 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatusToday");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatusByDate");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRequestTypeCountToday");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRequestTypeCountByDate");

            this.DropObjectIfExists(DropObjectType.Function, "Common", "udf_GetEndOfDay");
            this.DropObjectIfExists(DropObjectType.Function, "Common", "udf_GetUserTimeZoneId");

            Sql(@"CREATE INDEX IDX_AccessUsers_UserTimeZoneId ON Access.Users (UserGuid) INCLUDE (TimeZoneInfoId);");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatusToday");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatusByDate");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRequestTypeCountToday");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRequestTypeCountByDate");

            this.DropObjectIfExists(DropObjectType.Function, "Common", "udf_GetEndOfDay");
            this.DropObjectIfExists(DropObjectType.Function, "Common", "udf_GetUserTimeZoneId");

            Sql(@"DROP INDEX IDX_AccessUsers_UserTimeZoneId ON Access.Users;");
        }
    }
}