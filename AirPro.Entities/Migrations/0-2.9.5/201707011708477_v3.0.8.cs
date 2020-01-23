namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v308 : DbMigration
    {
        public override void Up()
        {
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwNotifications') DROP VIEW [Access].[vwNotifications];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwUserMemberships') DROP VIEW [Access].[vwUserMemberships];");

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwInvoiceBalances') DROP VIEW [Billing].[vwInvoiceBalances];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwInvoiceByShop') DROP VIEW [Billing].[vwInvoiceByShop];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwPaymentTransactions') DROP VIEW [Billing].[vwPaymentTransactions];");

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwRequestTimes') DROP VIEW [Repair].[vwRequestTimes];");

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwDTCResultsByScan') DROP VIEW [Scan].[vwDTCResultsByScan];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwRequestsByShop') DROP VIEW [Scan].[vwRequestsByShop];");

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Access' AND ROUTINE_NAME = 'usp_NotificationUsers') DROP PROCEDURE [Access].[usp_NotificationUsers];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Notification' AND ROUTINE_NAME = 'usp_GetNotificationUsers') DROP PROCEDURE [Notification].[usp_GetNotificationUsers];");

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Access' AND ROUTINE_NAME = 'usp_ViewAdminRights') DROP PROCEDURE [Access].[usp_ViewAdminRights];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Access' AND ROUTINE_NAME = 'usp_ViewUserGroupRoles') DROP PROCEDURE [Access].[usp_ViewUserGroupRoles];");

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetOutstandingInvoices') DROP PROCEDURE [Billing].[usp_GetOutstandingInvoices];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetPaymentTransactions') DROP PROCEDURE [Billing].[usp_GetPaymentTransactions];");
        }

        public override void Down()
        {
        }
    }
}
