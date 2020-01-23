CREATE TABLE [Access].[RegistrationUsers] (
    [RegistrationUserId]      INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]               NVARCHAR (256) NULL,
    [LastName]                NVARCHAR (256) NULL,
    [JobTitle]                NVARCHAR (256) NULL,
    [ContactNumber]           NVARCHAR (50)  NULL,
    [PhoneNumber]             NVARCHAR (50)  NULL,
    [AccessGroupIds]          NVARCHAR (MAX) NULL,
    [ShopBillingNotification] BIT            NOT NULL,
    [ShopReportNotification]  BIT            NOT NULL,
    [TimeZoneInfoId]          NVARCHAR (128) NULL,
    [PasswordHash]            NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Access.RegistrationUsers] PRIMARY KEY CLUSTERED ([RegistrationUserId] ASC)
);

