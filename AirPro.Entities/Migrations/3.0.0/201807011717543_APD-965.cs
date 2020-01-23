namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD965 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "CurrencyId", c => c.Int(nullable: false, defaultValueSql: "1"));
            CreateIndex("Access.Shops", "CurrencyId");
            AddForeignKey("Access.Shops", "CurrencyId", "Billing.Currencies", "CurrencyId");
        }

        public override void Down()
        {
            DropForeignKey("Access.Shops", "CurrencyId", "Billing.Currencies");
            DropIndex("Access.Shops", new[] { "CurrencyId" });
            DropColumn("Access.Shops", "CurrencyId");
        }
    }
}
