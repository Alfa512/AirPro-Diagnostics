CREATE TABLE [Billing].[ShopInsuranceCompaniesPricing] (
    [ShopId]             UNIQUEIDENTIFIER NOT NULL,
    [InsuranceCompanyId] INT              NOT NULL,
    [PricingPlanId]      INT              NOT NULL,
    CONSTRAINT [PK_Access.ShopInsuranceCompanies] PRIMARY KEY CLUSTERED ([ShopId] ASC, [InsuranceCompanyId] ASC),
    CONSTRAINT [FK_Access.ShopInsuranceCompanies_Access.Shops_ShopId] FOREIGN KEY ([ShopId]) REFERENCES [Access].[Shops] ([ShopGuid]),
    CONSTRAINT [FK_Access.ShopInsuranceCompanies_Billing.PricingPlans_PricingPlanId] FOREIGN KEY ([PricingPlanId]) REFERENCES [Billing].[PricingPlans] ([PricingPlanId]),
    CONSTRAINT [FK_Access.ShopInsuranceCompanies_Repair.InsuranceCompanies_InsuranceCompanyId] FOREIGN KEY ([InsuranceCompanyId]) REFERENCES [Repair].[InsuranceCompanies] ([InsuranceCompanyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PricingPlanId]
    ON [Billing].[ShopInsuranceCompaniesPricing]([PricingPlanId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InsuranceCompanyId]
    ON [Billing].[ShopInsuranceCompaniesPricing]([InsuranceCompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopId]
    ON [Billing].[ShopInsuranceCompaniesPricing]([ShopId] ASC);

