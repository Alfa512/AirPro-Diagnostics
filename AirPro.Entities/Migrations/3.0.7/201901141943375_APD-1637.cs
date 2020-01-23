namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1637 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolAccountsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolShopsArchive");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolAccountsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolShopsArchive");
        }
    }
}
