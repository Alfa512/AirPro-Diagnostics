using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_9 : DbMigration
    {
        public override void Up()
        {
            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201609031817284_v2_7_9.sql");
        }

        public override void Down()
        {
        }
    }
}
