namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Users", "TimeZoneInfoId", c => c.String());

            Sql("UPDATE Access.Users SET TimeZoneInfoId = 'Eastern Standard Time';");
        }
        
        public override void Down()
        {
            DropColumn("Access.Users", "TimeZoneInfoId");
        }
    }
}
