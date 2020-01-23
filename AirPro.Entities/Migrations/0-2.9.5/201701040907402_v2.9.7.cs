using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v297 : DbMigration
    {
        public override void Up()
        {

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201701040907402_v2.9.7_Deploy.sql");

        }

        public override void Down()
        {
        }
    }
}
