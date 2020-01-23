namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD428 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Accounts", "ActiveInd", c => c.Boolean(nullable: false, defaultValueSql: "1"));
            AddColumn("Access.Shops", "ActiveInd", c => c.Boolean(nullable: false, defaultValueSql: "1"));
        }

        public override void Down()
        {
            DropColumn("Access.Shops", "ActiveInd");
            DropColumn("Access.Accounts", "ActiveInd");
        }
    }
}
