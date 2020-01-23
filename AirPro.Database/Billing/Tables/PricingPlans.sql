CREATE TABLE [Billing].[PricingPlans] (
    [PricingPlanId]          INT                IDENTITY (1, 1) NOT NULL,
    [PricingPlanName]        NVARCHAR (MAX)     NULL,
    [PricingPlanDescription] NVARCHAR (MAX)     NULL,
    [PricingPlanActiveInd]   BIT                DEFAULT ((1)) NOT NULL,
    [CreatedByUserGuid]      UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]              DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]      UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]              DATETIMEOFFSET (7) NULL,
    [CurrencyId]             INT                NULL,
    CONSTRAINT [PK_Billing.PricingPlans] PRIMARY KEY CLUSTERED ([PricingPlanId] ASC),
    CONSTRAINT [FK_Billing.PricingPlans_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Billing.PricingPlans_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Billing.PricingPlans_Billing.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [Billing].[Currencies] ([CurrencyId])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Billing].[PricingPlans]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Billing].[PricingPlans]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CurrencyId]
    ON [Billing].[PricingPlans]([CurrencyId] ASC);

