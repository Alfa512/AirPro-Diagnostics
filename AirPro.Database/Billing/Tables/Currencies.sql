CREATE TABLE [Billing].[Currencies] (
    [CurrencyId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Billing.Currencies] PRIMARY KEY CLUSTERED ([CurrencyId] ASC)
);

