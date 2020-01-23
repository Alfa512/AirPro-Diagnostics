CREATE TABLE [Billing].[ShopInsuranceCompaniesEstimate] (
    [ShopId]             UNIQUEIDENTIFIER NOT NULL,
    [InsuranceCompanyId] INT              NOT NULL,
    [EstimatePlanId]     INT              NOT NULL,
    CONSTRAINT [PK_Billing.ShopInsuranceCompaniesEstimate] PRIMARY KEY CLUSTERED ([ShopId] ASC, [InsuranceCompanyId] ASC),
    CONSTRAINT [FK_Billing.ShopInsuranceCompaniesEstimate_Access.Shops_ShopId] FOREIGN KEY ([ShopId]) REFERENCES [Access].[Shops] ([ShopGuid]) ON DELETE CASCADE,
    CONSTRAINT [FK_Billing.ShopInsuranceCompaniesEstimate_Billing.EstimatePlans_EstimatePlanId] FOREIGN KEY ([EstimatePlanId]) REFERENCES [Billing].[EstimatePlans] ([EstimatePlanId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Billing.ShopInsuranceCompaniesEstimate_Repair.InsuranceCompanies_InsuranceCompanyId] FOREIGN KEY ([InsuranceCompanyId]) REFERENCES [Repair].[InsuranceCompanies] ([InsuranceCompanyId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_EstimatePlanId]
    ON [Billing].[ShopInsuranceCompaniesEstimate]([EstimatePlanId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InsuranceCompanyId]
    ON [Billing].[ShopInsuranceCompaniesEstimate]([InsuranceCompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopId]
    ON [Billing].[ShopInsuranceCompaniesEstimate]([ShopId] ASC);

