using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD281 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Users", "ShopStatementNotification", c => c.Boolean(nullable: false));
            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetUserMemberships");

            Sql(@"SET IDENTITY_INSERT Notification.Templates ON;

                MERGE Notification.Templates AS t
                USING
                (
	                SELECT
		                5 [NotificationTemplateId]
		                ,'ShopStatementEmail' [Name]
		                ,'PaymentID,ShopName,StatementLink,PaymentAmount,PaymentReferenceNumber,PaymentMemo,PaymentType,PaymentCurrency,DiscountPercentage' [Options]
		                ,'New Payment Statement (ID {PaymentId})' [Subject]
		                ,'<p>Payment ID {PaymentID} is ready to be viewed.</p>' [EmailBody]
                ) AS s
                ON (s.NotificationTemplateId = t.NotificationTemplateId)
                WHEN NOT MATCHED THEN
	                INSERT (NotificationTemplateId, Name, Options, Subject, EmailBody, CreatedDt, CreatedByUserGuid)
	                VALUES (NotificationTemplateId, Name, Options, Subject, EmailBody, GETUTCDATE(), Common.udf_GetEmptyGuid())
                OUTPUT INSERTED.*;

                SET IDENTITY_INSERT Notification.Templates OFF;

                DBCC CHECKIDENT('Notification.Templates', RESEED);");
        }
        
        public override void Down()
        {
            DropColumn("Access.Users", "ShopStatementNotification");
            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetUserMemberships");

            Sql("DELETE FROM [Notification].[Templates] WHERE NotificationTemplateId = 5;" +
                "DBCC CHECKIDENT('Notification.Templates', RESEED);");
        }
    }
}
