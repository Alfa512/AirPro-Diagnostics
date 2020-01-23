namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD317 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Support.ApplicationExceptions", "ExceptionParentId", c => c.Int());
            CreateIndex("Support.ApplicationExceptions", "ExceptionParentId");
        }

        public override void Down()
        {
            DropIndex("Support.ApplicationExceptions", new[] { "ExceptionParentId" });
            DropColumn("Support.ApplicationExceptions", "ExceptionParentId");
        }
    }
}
