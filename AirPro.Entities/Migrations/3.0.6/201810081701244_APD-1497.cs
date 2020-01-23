using System.Data.Entity.Migrations;
using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    public partial class APD1497 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "DisableShopBillingNotification", c => c.Boolean(nullable: false));
            AddColumn("Access.Shops", "DisableShopStatementNotification", c => c.Boolean(nullable: false));
            AddColumn("Access.ShopsArchive", "DisableShopBillingNotification", c => c.Boolean(nullable: false));
            AddColumn("Access.ShopsArchive", "DisableShopStatementNotification", c => c.Boolean(nullable: false));

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetUserMemberships");
        }
        
        public override void Down()
        {
            DropColumn("Access.ShopsArchive", "DisableShopStatementNotification");
            DropColumn("Access.ShopsArchive", "DisableShopBillingNotification");
            DropColumn("Access.Shops", "DisableShopStatementNotification");
            DropColumn("Access.Shops", "DisableShopBillingNotification");

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetUserMemberships");
        }
    }
}
