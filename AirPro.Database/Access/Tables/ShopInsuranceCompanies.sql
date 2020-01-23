CREATE TABLE [Access].[ShopInsuranceCompanies] (
    [ShopId]             UNIQUEIDENTIFIER NOT NULL,
    [InsuranceCompanyId] INT              NOT NULL,
    CONSTRAINT [PK_Access.ShopInsuranceCompanies] PRIMARY KEY CLUSTERED ([ShopId] ASC, [InsuranceCompanyId] ASC),
    CONSTRAINT [FK_Access.ShopInsuranceCompanies_Access.Shops_ShopId] FOREIGN KEY ([ShopId]) REFERENCES [Access].[Shops] ([ShopGuid]),
    CONSTRAINT [FK_Access.ShopInsuranceCompanies_Repair.InsuranceCompanies_InsuranceCompanyId] FOREIGN KEY ([InsuranceCompanyId]) REFERENCES [Repair].[InsuranceCompanies] ([InsuranceCompanyId])
);




GO



GO
CREATE NONCLUSTERED INDEX [IX_InsuranceCompanyId]
    ON [Access].[ShopInsuranceCompanies]([InsuranceCompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopId]
    ON [Access].[ShopInsuranceCompanies]([ShopId] ASC);

