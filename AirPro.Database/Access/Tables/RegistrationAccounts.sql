CREATE TABLE [Access].[RegistrationAccounts] (
    [RegistrationAccountId] INT             IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (256)  NULL,
    [Phone]                 NVARCHAR (15)   NULL,
    [Fax]                   NVARCHAR (15)   NULL,
    [Address1]              NVARCHAR (1024) NULL,
    [Address2]              NVARCHAR (1024) NULL,
    [City]                  NVARCHAR (1024) NULL,
    [StateId]               NVARCHAR (MAX)  NULL,
    [Zip]                   NVARCHAR (25)   NULL,
    [DiscountPercentage]    INT             NULL,
    CONSTRAINT [PK_Access.RegistrationAccounts] PRIMARY KEY CLUSTERED ([RegistrationAccountId] ASC)
);

