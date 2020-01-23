CREATE TABLE [Access].[ShopsArchive] (
    [ArchiveId]                        INT                IDENTITY (1, 1) NOT NULL,
    [ArchiveDt]                        DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [AccountGuid]                      UNIQUEIDENTIFIER   NOT NULL,
    [ActiveInd]                        BIT                NOT NULL,
    [AdditionalScanCost]               DECIMAL (18, 2)    NOT NULL,
    [Address1]                         NVARCHAR (MAX)     NULL,
    [Address2]                         NVARCHAR (MAX)     NULL,
    [AllowAutoRepairClose]             BIT                NOT NULL,
    [AllowDemoScan]                    BIT                NOT NULL,
    [AllowScanAnalysis]                BIT                NOT NULL,
    [AllowSelfScan]                    BIT                NOT NULL,
    [AllowSelfScanAssessment]          BIT                NOT NULL,
    [AutomaticRepairCloseDays]         INT                NULL,
    [AverageVehiclesPerMonth]          INT                NULL,
    [CCCShopId]                        NVARCHAR (MAX)     NULL,
    [City]                             NVARCHAR (MAX)     NULL,
    [CurrencyId]                       INT                NOT NULL,
    [DefaultInsuranceCompanyId]        INT                NULL,
    [DiscountPercentage]               INT                NOT NULL,
    [EstimatePlanId]                   INT                NULL,
    [Fax]                              NVARCHAR (MAX)     NULL,
    [FirstScanCost]                    DECIMAL (18, 2)    NOT NULL,
    [HideFromReports]                  BIT                NOT NULL,
    [Name]                             NVARCHAR (MAX)     NULL,
    [Notes]                            NVARCHAR (MAX)     NULL,
    [Phone]                            NVARCHAR (MAX)     NULL,
    [PricingPlanId]                    INT                NULL,
    [ShopFixedPriceInd]                BIT                NOT NULL,
    [ShopGuid]                         UNIQUEIDENTIFIER   NOT NULL,
    [ShopNumber]                       INT                NOT NULL,
    [StateId]                          INT                NOT NULL,
    [Zip]                              NVARCHAR (MAX)     NULL,
    [CreatedByUserGuid]                UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]                        DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]                UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]                        DATETIMEOFFSET (7) NULL,
    [SendToMitchellInd]                BIT                DEFAULT ((0)) NOT NULL,
    [DisableShopBillingNotification]   BIT                DEFAULT ((0)) NOT NULL,
    [DisableShopStatementNotification] BIT                DEFAULT ((0)) NOT NULL,
    [AllowAllRepairAutoClose]          BIT                DEFAULT ((0)) NOT NULL,
    [AllowScanAnalysisAutoClose]       BIT                DEFAULT ((0)) NOT NULL,
    [BillingCycleId]                   INT                NULL,
    [EmployeeGuid]                     UNIQUEIDENTIFIER   NULL,
    [AutomaticInvoicesInd]             BIT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Access.ShopsArchive] PRIMARY KEY CLUSTERED ([ArchiveId] ASC)
);







