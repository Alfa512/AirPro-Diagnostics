namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v293 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Access.UserClaims", name: "UserId", newName: "UserGuid");
            RenameIndex(table: "Access.UserClaims", name: "IX_UserId", newName: "IX_UserGuid");
            RenameColumn(table: "Support.ApplicationExceptions", name: "ExceptionDateTime", newName: "ExceptionOccuredDt");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Support.ApplicationExceptions", name: "ExceptionOccuredDt", newName: "ExceptionDateTime");
            RenameIndex(table: "Access.UserClaims", name: "IX_UserGuid", newName: "IX_UserId");
            RenameColumn(table: "Access.UserClaims", name: "UserGuid", newName: "UserId");
        }
    }
}
