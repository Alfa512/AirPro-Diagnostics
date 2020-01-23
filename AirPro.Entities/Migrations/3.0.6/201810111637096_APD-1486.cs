using UniMatrix.Common.Enumerations;

namespace AirPro.Entities.Migrations
{
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Extensions;

    public partial class APD1486 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Access.ShopContacts",
                c => new
                    {
                        ShopContactGuid = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        ShopGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ShopContactGuid)
                .ForeignKey("Access.Shops", t => t.ShopGuid, cascadeDelete: true)
                .Index(t => t.ShopGuid);
            
            AddColumn("Scan.Requests", "ShopContactGuid", c => c.Guid());
            CreateIndex("Scan.Requests", "ShopContactGuid");
            AddForeignKey("Scan.Requests", "ShopContactGuid", "Access.ShopContacts", "ShopContactGuid");

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetUsersByRepairId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetTechnicianRequest");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestById");
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.Requests", "ShopContactGuid", "Access.ShopContacts");
            DropForeignKey("Access.ShopContacts", "ShopGuid", "Access.Shops");
            DropIndex("Scan.Requests", new[] { "ShopContactGuid" });
            DropIndex("Access.ShopContacts", new[] { "ShopGuid" });
            DropColumn("Scan.Requests", "ShopContactGuid");
            DropTable("Access.ShopContacts");

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetUsersByRepairId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetTechnicianRequest");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestById");
        }
    }
}
