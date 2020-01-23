CREATE TABLE [Access].[RegistrationShops] (
    [RegistrationShopId]                  INT             IDENTITY (1, 1) NOT NULL,
    [Name]                                NVARCHAR (256)  NULL,
    [Phone]                               NVARCHAR (15)   NULL,
    [Fax]                                 NVARCHAR (15)   NULL,
    [Address1]                            NVARCHAR (1024) NULL,
    [Address2]                            NVARCHAR (1024) NULL,
    [City]                                NVARCHAR (1024) NULL,
    [StateId]                             NVARCHAR (MAX)  NULL,
    [Zip]                                 NVARCHAR (25)   NULL,
    [SendToMitchellInd]                   BIT             NOT NULL,
    [DiscountPercentage]                  INT             NULL,
    [CCCShopId]                           NVARCHAR (128)  NULL,
    [AllowAllRepairAutoClose]             BIT             NOT NULL,
    [AllowAutoRepairClose]                BIT             NOT NULL,
    [AllowScanAnalysisAutoClose]          BIT             NOT NULL,
    [ShopFixedPriceInd]                   BIT             NOT NULL,
    [FirstScanCost]                       DECIMAL (18, 2) NULL,
    [AdditionalScanCost]                  DECIMAL (18, 2) NULL,
    [AutomaticRepairCloseDays]            INT             NULL,
    [HideFromReports]                     BIT             NOT NULL,
    [DisableShopBillingNotification]      BIT             NOT NULL,
    [DisableShopStatementNotification]    BIT             NOT NULL,
    [DefaultInsuranceCompanyId]           INT             NULL,
    [AverageVehiclesPerMonth]             INT             NULL,
    [CurrencyId]                          INT             NULL,
    [PricingPlanId]                       INT             NULL,
    [EstimatePlanId]                      INT             NULL,
    [BillingCycleId]                      INT             NULL,
    [AllowedRequestTypeIds]               NVARCHAR (MAX)  NULL,
    [InsuranceCompaniesIds]               NVARCHAR (MAX)  NULL,
    [InsuranceCompaniesPricingPlansJson]  NVARCHAR (MAX)  NULL,
    [InsuranceCompaniesEstimatePlansJson] NVARCHAR (MAX)  NULL,
    [VehicleMakesIds]                     NVARCHAR (MAX)  NULL,
    [VehicleMakesPricingPlansJson]        NVARCHAR (MAX)  NULL,
    [AllowSelfScanAssessment]             BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Access.RegistrationShops] PRIMARY KEY CLUSTERED ([RegistrationShopId] ASC),
    CONSTRAINT [FK_Access.RegistrationShops_Billing.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [Billing].[Currencies] ([CurrencyId]),
    CONSTRAINT [FK_Access.RegistrationShops_Billing.Cycles_BillingCycleId] FOREIGN KEY ([BillingCycleId]) REFERENCES [Billing].[Cycles] ([CycleId]),
    CONSTRAINT [FK_Access.RegistrationShops_Billing.EstimatePlans_EstimatePlanId] FOREIGN KEY ([EstimatePlanId]) REFERENCES [Billing].[EstimatePlans] ([EstimatePlanId]),
    CONSTRAINT [FK_Access.RegistrationShops_Billing.PricingPlans_PricingPlanId] FOREIGN KEY ([PricingPlanId]) REFERENCES [Billing].[PricingPlans] ([PricingPlanId]),
    CONSTRAINT [FK_Access.RegistrationShops_Repair.InsuranceCompanies_DefaultInsuranceCompanyId] FOREIGN KEY ([DefaultInsuranceCompanyId]) REFERENCES [Repair].[InsuranceCompanies] ([InsuranceCompanyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BillingCycleId]
    ON [Access].[RegistrationShops]([BillingCycleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EstimatePlanId]
    ON [Access].[RegistrationShops]([EstimatePlanId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PricingPlanId]
    ON [Access].[RegistrationShops]([PricingPlanId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CurrencyId]
    ON [Access].[RegistrationShops]([CurrencyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DefaultInsuranceCompanyId]
    ON [Access].[RegistrationShops]([DefaultInsuranceCompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CCCShopId]
    ON [Access].[RegistrationShops]([CCCShopId] ASC);

