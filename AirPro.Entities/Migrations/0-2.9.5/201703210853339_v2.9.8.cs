using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v298 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Access.Groups", "GroupGuid", c => c.Guid(nullable: false, identity: true, defaultValueSql: "NEWSEQUENTIALID()"));

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201703210853339_v2.9.8_Deploy.sql");
        }

        public override void Down()
        {

        }
    }
}
