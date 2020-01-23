CREATE TABLE [Billing].[Payments] (
    [PaymentID]                   INT                IDENTITY (1, 1) NOT NULL,
    [PaymentReceivedFromShopGuid] UNIQUEIDENTIFIER   NOT NULL,
    [PaymentTypeId]               INT                NOT NULL,
    [PaymentAmount]               DECIMAL (18, 2)    NOT NULL,
    [PaymentReferenceNumber]      NVARCHAR (MAX)     NULL,
    [PaymentMemo]                 NVARCHAR (MAX)     NULL,
    [PaymentVoidInd]              BIT                NOT NULL,
    [PaymentVoidByUserGuid]       UNIQUEIDENTIFIER   NULL,
    [PaymentVoidDt]               DATETIMEOFFSET (7) NULL,
    [CreatedDt]                   DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDt]                   DATETIMEOFFSET (7) NULL,
    [CreatedByUserGuid]           UNIQUEIDENTIFIER   NOT NULL,
    [UpdatedByUserGuid]           UNIQUEIDENTIFIER   NULL,
    [DiscountPercentage]          INT                DEFAULT ((0)) NOT NULL,
    [CurrencyId]                  INT                NULL,
    [PaymentDt]                   DATE               NULL,
    CONSTRAINT [PK_Billing.Payments] PRIMARY KEY CLUSTERED ([PaymentID] ASC),
    CONSTRAINT [FK_Billing.Payments_Access.Shops_PaymentReceivedFromShopGuid] FOREIGN KEY ([PaymentReceivedFromShopGuid]) REFERENCES [Access].[Shops] ([ShopGuid]),
    CONSTRAINT [FK_Billing.Payments_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Billing.Payments_Access.Users_PaymentVoidByUserGuid] FOREIGN KEY ([PaymentVoidByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Billing.Payments_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Billing.Payments_Billing.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [Billing].[Currencies] ([CurrencyId]),
    CONSTRAINT [FK_Billing.Payments_Billing.PaymentTypes_PaymentTypeID] FOREIGN KEY ([PaymentTypeId]) REFERENCES [Billing].[PaymentTypes] ([PaymentTypeId])
);










GO



GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_PaymentTypeID]
    ON [Billing].[Payments]([PaymentTypeID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Billing].[Payments]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PaymentVoidByUserGuid]
    ON [Billing].[Payments]([PaymentVoidByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PaymentReceivedFromShopGuid]
    ON [Billing].[Payments]([PaymentReceivedFromShopGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Billing].[Payments]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CurrencyId]
    ON [Billing].[Payments]([CurrencyId] ASC);

