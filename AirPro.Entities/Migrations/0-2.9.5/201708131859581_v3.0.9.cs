namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v309 : DbMigration
    {
        public override void Up()
        {
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetOutstandingInvoices') DROP PROCEDURE [Billing].[usp_GetOutstandingInvoices];");
        }

        public override void Down()
        {
        }
    }
}
