using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v287 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Access.Users", "Shop_ID", "Access.Shops");
            DropForeignKey("Repair.Orders", "Shop_ID", "Access.Shops");
            DropForeignKey("Billing.Payments", "PaymentReceivedFromShopID", "Access.Shops");
            DropForeignKey("Access.UserShops", "ShopId", "Access.Shops");
            DropIndex("Access.Users", new[] { "Shop_ID" });
            DropIndex("Repair.Orders", new[] { "Shop_ID" });
            DropPrimaryKey("Access.Shops");

            RenameColumn("Access.Shops", "ID", "ShopId");
            RenameColumn(table: "Access.Users", name: "Shop_ID", newName: "ShopId");
            RenameColumn(table: "Repair.Orders", name: "Shop_ID", newName: "ShopId");
            AlterColumn("Access.Users", "ShopId", c => c.Int(nullable: false));
            AlterColumn("Repair.Orders", "ShopId", c => c.Int(nullable: false));

            AddPrimaryKey("Access.Shops", "ShopId");
            CreateIndex("Access.Users", "ShopId");
            CreateIndex("Repair.Orders", "ShopId");
            AddForeignKey("Access.Users", "ShopId", "Access.Shops", "ShopId");
            AddForeignKey("Repair.Orders", "ShopId", "Access.Shops", "ShopId");
            AddForeignKey("Billing.Payments", "PaymentReceivedFromShopID", "Access.Shops", "ShopId");
            AddForeignKey("Access.UserShops", "ShopId", "Access.Shops", "ShopId");

            RenameColumn(table: "Access.Accounts", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Access.Accounts", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Repair.Orders", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Repair.Orders", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Repair.Invoices", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Repair.Invoices", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Repair.Photos", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Repair.Photos", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Scan.Requests", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Scan.Requests", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Scan.Reports", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Scan.Reports", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Scan.ReportsArchive", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Scan.ReportsArchive", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Scan.RequestTypes", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Scan.RequestTypes", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Scan.Uploads", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Scan.Uploads", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Access.GroupRoles", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Access.GroupRoles", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Access.Groups", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Access.Groups", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Support.NotificationTemplates", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Support.NotificationTemplates", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Billing.Payments", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Billing.Payments", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Billing.PaymentTransactions", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Billing.PaymentTransactions", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Access.UserAccounts", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Access.UserAccounts", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Access.UserGroups", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Access.UserGroups", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameColumn(table: "Access.UserShops", name: "CreatedBy_Id", newName: "CreatedByUserId");
            RenameColumn(table: "Access.UserShops", name: "UpdatedBy_Id", newName: "UpdatedByUserId");
            RenameIndex(table: "Access.Accounts", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Access.Accounts", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Repair.Orders", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Repair.Orders", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Repair.Invoices", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Repair.Invoices", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Repair.Photos", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Repair.Photos", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Scan.Requests", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Scan.Requests", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Scan.Reports", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Scan.Reports", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Scan.RequestTypes", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Scan.RequestTypes", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Scan.Uploads", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Scan.Uploads", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Access.GroupRoles", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Access.GroupRoles", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Access.Groups", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Access.Groups", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Support.NotificationTemplates", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Support.NotificationTemplates", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Billing.Payments", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Billing.Payments", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Billing.PaymentTransactions", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Billing.PaymentTransactions", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Access.UserAccounts", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Access.UserAccounts", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Access.UserGroups", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Access.UserGroups", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");
            RenameIndex(table: "Access.UserShops", name: "IX_CreatedBy_Id", newName: "IX_CreatedByUserId");
            RenameIndex(table: "Access.UserShops", name: "IX_UpdatedBy_Id", newName: "IX_UpdatedByUserId");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612281952513_v2.8.7_Deploy.sql");
        }

        public override void Down()
        {
            DropForeignKey("Access.UserShops", "ShopId", "Access.Shops");
            DropForeignKey("Billing.Payments", "PaymentReceivedFromShopID", "Access.Shops");
            DropForeignKey("Repair.Orders", "ShopId", "Access.Shops");
            DropForeignKey("Access.Users", "ShopId", "Access.Shops");
            DropIndex("Repair.Orders", new[] { "ShopId" });
            DropIndex("Access.Users", new[] { "ShopId" });
            DropPrimaryKey("Access.Shops");

            RenameColumn("Access.Shops", "ShopId", "ID");
            RenameColumn(table: "Repair.Orders", name: "ShopId", newName: "Shop_ID");
            RenameColumn(table: "Access.Users", name: "ShopId", newName: "Shop_ID");
            AlterColumn("Repair.Orders", "Shop_ID", c => c.Int());
            AlterColumn("Access.Users", "Shop_ID", c => c.Int());

            AddPrimaryKey("Access.Shops", "ID");
            CreateIndex("Repair.Orders", "Shop_ID");
            CreateIndex("Access.Users", "Shop_ID");
            AddForeignKey("Access.UserShops", "ShopId", "Access.Shops", "ID");
            AddForeignKey("Billing.Payments", "PaymentReceivedFromShopID", "Access.Shops", "ID");
            AddForeignKey("Repair.Orders", "Shop_ID", "Access.Shops", "ID");
            AddForeignKey("Access.Users", "Shop_ID", "Access.Shops", "ID");

            RenameIndex(table: "Access.UserShops", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Access.UserShops", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Access.UserGroups", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Access.UserGroups", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Access.UserAccounts", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Access.UserAccounts", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Billing.PaymentTransactions", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Billing.PaymentTransactions", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Billing.Payments", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Billing.Payments", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Support.NotificationTemplates", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Support.NotificationTemplates", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Access.Groups", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Access.Groups", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Access.GroupRoles", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Access.GroupRoles", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Scan.Uploads", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Scan.Uploads", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Scan.RequestTypes", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Scan.RequestTypes", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Scan.Reports", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Scan.Reports", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Scan.Requests", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Scan.Requests", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Repair.Photos", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Repair.Photos", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Repair.Invoices", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Repair.Invoices", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Repair.Orders", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Repair.Orders", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameIndex(table: "Access.Accounts", name: "IX_UpdatedByUserId", newName: "IX_UpdatedBy_Id");
            RenameIndex(table: "Access.Accounts", name: "IX_CreatedByUserId", newName: "IX_CreatedBy_Id");
            RenameColumn(table: "Access.UserShops", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Access.UserShops", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Access.UserGroups", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Access.UserGroups", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Access.UserAccounts", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Access.UserAccounts", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Billing.PaymentTransactions", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Billing.PaymentTransactions", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Billing.Payments", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Billing.Payments", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Support.NotificationTemplates", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Support.NotificationTemplates", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Access.Groups", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Access.Groups", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Access.GroupRoles", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Access.GroupRoles", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Scan.Uploads", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Scan.Uploads", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Scan.RequestTypes", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Scan.RequestTypes", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Scan.Reports", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Scan.Reports", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Scan.ReportsArchive", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Scan.ReportsArchive", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Scan.Requests", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Scan.Requests", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Repair.Photos", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Repair.Photos", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Repair.Invoices", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Repair.Invoices", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Repair.Orders", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Repair.Orders", name: "CreatedByUserId", newName: "CreatedBy_Id");
            RenameColumn(table: "Access.Accounts", name: "UpdatedByUserId", newName: "UpdatedBy_Id");
            RenameColumn(table: "Access.Accounts", name: "CreatedByUserId", newName: "CreatedBy_Id");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612281952513_v2.8.7_Rollback.sql");

        }
    }
}
