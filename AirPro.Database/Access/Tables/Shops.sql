CREATE TABLE [Access].[Shops] (
    [ShopId]                           INT                IDENTITY (1, 1) NOT NULL,
    [ShopGuid]                         UNIQUEIDENTIFIER   CONSTRAINT [DF_Access.Shops_ShopGuid] DEFAULT (newsequentialid()) NOT NULL,
    [AccountGuid]                      UNIQUEIDENTIFIER   NOT NULL,
    [Name]                             NVARCHAR (MAX)     NOT NULL,
    [Phone]                            NVARCHAR (MAX)     NULL,
    [Fax]                              NVARCHAR (MAX)     NULL,
    [Address1]                         NVARCHAR (MAX)     NULL,
    [Address2]                         NVARCHAR (MAX)     NULL,
    [City]                             NVARCHAR (MAX)     NULL,
    [StateId]                          INT                NOT NULL,
    [Zip]                              NVARCHAR (MAX)     NULL,
    [Notes]                            NVARCHAR (MAX)     NULL,
    [DiscountPercentage]               INT                DEFAULT ((0)) NOT NULL,
    [CCCShopId]                        NVARCHAR (128)     NULL,
    [AllowSelfScan]                    BIT                DEFAULT ((0)) NOT NULL,
    [PricingPlanId]                    INT                NULL,
    [AverageVehiclesPerMonth]          INT                NULL,
    [EstimatePlanId]                   INT                NULL,
    [AllowAutoRepairClose]             BIT                DEFAULT ((0)) NOT NULL,
    [AllowScanAnalysis]                BIT                DEFAULT ((0)) NOT NULL,
    [DefaultInsuranceCompanyId]        INT                NULL,
    [AllowSelfScanAssessment]          BIT                DEFAULT ((0)) NOT NULL,
    [ShopFixedPriceInd]                BIT                DEFAULT ((0)) NOT NULL,
    [FirstScanCost]                    DECIMAL (18, 2)    DEFAULT ((0)) NOT NULL,
    [AdditionalScanCost]               DECIMAL (18, 2)    DEFAULT ((0)) NOT NULL,
    [AutomaticRepairCloseDays]         INT                NULL,
    [ActiveInd]                        BIT                DEFAULT ((1)) NOT NULL,
    [HideFromReports]                  BIT                DEFAULT ((0)) NOT NULL,
    [ShopNumber]                       AS                 ([ShopId]+(10000)),
    [AllowDemoScan]                    BIT                DEFAULT ((0)) NOT NULL,
    [CurrencyId]                       INT                DEFAULT ((1)) NOT NULL,
    [CreatedByUserGuid]                UNIQUEIDENTIFIER   DEFAULT ([Common].[udf_GetEmptyGuid]()) NOT NULL,
    [CreatedDt]                        DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [UpdatedByUserGuid]                UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                        DATETIMEOFFSET (7) NULL,
    [BillingCycleId]                   INT                NULL,
    [AllowScanAnalysisAutoClose]       BIT                DEFAULT ((0)) NOT NULL,
    [SendToMitchellInd]                BIT                DEFAULT ((0)) NOT NULL,
    [AllowAllRepairAutoClose]          BIT                DEFAULT ((0)) NOT NULL,
    [DisableShopBillingNotification]   BIT                DEFAULT ((0)) NOT NULL,
    [DisableShopStatementNotification] BIT                DEFAULT ((0)) NOT NULL,
    [EmployeeGuid]                     UNIQUEIDENTIFIER   NULL,
    [DisplayName]                      AS                 (CONVERT([nvarchar](200),(([Name]+' (')+CONVERT([nchar](5),[ShopId]+(10000)))+')')),
    [AutomaticInvoicesInd]             BIT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Access.Shops] PRIMARY KEY CLUSTERED ([ShopGuid] ASC),
    CONSTRAINT [FK_Access.Shops_Access.Accounts_AccountGuid] FOREIGN KEY ([AccountGuid]) REFERENCES [Access].[Accounts] ([AccountGuid]),
    CONSTRAINT [FK_Access.Shops_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Shops_Access.Users_EmployeeGuid] FOREIGN KEY ([EmployeeGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Shops_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Shops_Billing.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [Billing].[Currencies] ([CurrencyId]),
    CONSTRAINT [FK_Access.Shops_Billing.Cycles_BillingCycleId] FOREIGN KEY ([BillingCycleId]) REFERENCES [Billing].[Cycles] ([CycleId]),
    CONSTRAINT [FK_Access.Shops_Billing.EstimatePlans_EstimatePlanId] FOREIGN KEY ([EstimatePlanId]) REFERENCES [Billing].[EstimatePlans] ([EstimatePlanId]),
    CONSTRAINT [FK_Access.Shops_Billing.PricingPlans_PricingPlanId] FOREIGN KEY ([PricingPlanId]) REFERENCES [Billing].[PricingPlans] ([PricingPlanId]),
    CONSTRAINT [FK_Access.Shops_Common.States_StateId] FOREIGN KEY ([StateId]) REFERENCES [Common].[States] ([StateId]),
    CONSTRAINT [FK_Access.Shops_Repair.InsuranceCompanies_DefaultInsuranceCompanyId] FOREIGN KEY ([DefaultInsuranceCompanyId]) REFERENCES [Repair].[InsuranceCompanies] ([InsuranceCompanyId])
);




























GO
CREATE NONCLUSTERED INDEX [IX_AccountGuid]
    ON [Access].[Shops]([AccountGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StateId]
    ON [Access].[Shops]([StateId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CCCShopId]
    ON [Access].[Shops]([CCCShopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PricingPlanId]
    ON [Access].[Shops]([PricingPlanId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EstimatePlanId]
    ON [Access].[Shops]([EstimatePlanId] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_ShopGuid_ShopName]
    ON [Access].[Shops]([ShopGuid] ASC)
    INCLUDE([Name]);


GO
CREATE NONCLUSTERED INDEX [IX_DefaultInsuranceCompanyId]
    ON [Access].[Shops]([DefaultInsuranceCompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CurrencyId]
    ON [Access].[Shops]([CurrencyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[Shops]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[Shops]([CreatedByUserGuid] ASC);


GO
CREATE TRIGGER [Access].[trgAccessShopsArchive] ON [Access].[Shops]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.ShopsArchive
	(
		ShopGuid
		,AccountGuid
		,Name
		,Phone
		,Fax
		,Address1
		,Address2
		,City
		,StateId
		,Zip
		,Notes
		,DiscountPercentage
		,CCCShopId
		,AllowSelfScan
		,PricingPlanId
		,AverageVehiclesPerMonth
		,EstimatePlanId
		,AllowAutoRepairClose
		,AllowScanAnalysis
		,DefaultInsuranceCompanyId
		,AllowSelfScanAssessment
		,ShopFixedPriceInd
		,FirstScanCost
		,AdditionalScanCost
		,AutomaticRepairCloseDays
		,ActiveInd
		,HideFromReports
		,ShopNumber
		,AllowDemoScan
		,CurrencyId
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,BillingCycleId
		,AllowScanAnalysisAutoClose
		,SendToMitchellInd
		,AllowAllRepairAutoClose
		,DisableShopBillingNotification
		,DisableShopStatementNotification
		,EmployeeGuid
		,AutomaticInvoicesInd
	)
	SELECT
		ShopGuid
		,AccountGuid
		,Name
		,Phone
		,Fax
		,Address1
		,Address2
		,City
		,StateId
		,Zip
		,Notes
		,DiscountPercentage
		,CCCShopId
		,AllowSelfScan
		,PricingPlanId
		,AverageVehiclesPerMonth
		,EstimatePlanId
		,AllowAutoRepairClose
		,AllowScanAnalysis
		,DefaultInsuranceCompanyId
		,AllowSelfScanAssessment
		,ShopFixedPriceInd
		,FirstScanCost
		,AdditionalScanCost
		,AutomaticRepairCloseDays
		,ActiveInd
		,HideFromReports
		,ShopNumber
		,AllowDemoScan
		,CurrencyId
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,BillingCycleId
		,AllowScanAnalysisAutoClose
		,SendToMitchellInd
		,AllowAllRepairAutoClose
		,DisableShopBillingNotification
		,DisableShopStatementNotification
		,EmployeeGuid
		,AutomaticInvoicesInd
	FROM deleted

END
GO
CREATE NONCLUSTERED INDEX [IX_BillingCycleId]
    ON [Access].[Shops]([BillingCycleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeGuid]
    ON [Access].[Shops]([EmployeeGuid] ASC);

