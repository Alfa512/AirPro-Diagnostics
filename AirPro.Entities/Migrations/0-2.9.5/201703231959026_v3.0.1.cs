namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v301 : DbMigration
    {
        public override void Up()
        {
            // One-Way Update.
            DropForeignKey("Access.UserRoles", "RoleGuid", "Access.Roles");
            AddForeignKey("Access.UserRoles", "RoleGuid", "Access.Roles", "RoleGuid", cascadeDelete: true);

            DropForeignKey("Access.GroupRoles", "RoleGuid", "Access.Roles");
            AddForeignKey("Access.GroupRoles", "RoleGuid", "Access.Roles", "RoleGuid", cascadeDelete: true);
        }
        
        public override void Down()
        {
            // Do NOT Undo Change.
        }
    }
}
