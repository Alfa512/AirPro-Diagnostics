namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Users", "ContactNumber", c => c.String());

            Sql("UPDATE Access.Users SET ContactNumber = PhoneNumber, PhoneNumber = NULL;");
        }
        
        public override void Down()
        {
            Sql("UPDATE Access.Users SET PhoneNumber = ContactNumber;");

            DropColumn("Access.Users", "ContactNumber");
        }
    }
}
