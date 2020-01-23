namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1006 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Notification' AND ROUTINE_NAME = 'usp_GetRequestNotification') DROP PROCEDURE Notification.usp_GetRequestNotification;");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Notification' AND ROUTINE_NAME = 'usp_GetRequestNotification') DROP PROCEDURE Notification.usp_GetRequestNotification;");
        }
    }
}
