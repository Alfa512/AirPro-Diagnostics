using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1851 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.ShopContacts", "DeletedInd", c => c.Boolean(nullable: false));
            
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetUsersByRepairId");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetUsersByRepairId");

            DropColumn("Access.ShopContacts", "DeletedInd");
        }
    }
}
