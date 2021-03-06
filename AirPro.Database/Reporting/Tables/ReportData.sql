﻿CREATE TABLE [Reporting].[ReportData] (
    [DataGuid]                     UNIQUEIDENTIFIER   CONSTRAINT [DF_DataGuid] DEFAULT (newsequentialid()) NOT NULL,
    [DataLoadId]                   INT                NULL,
    [DataChecksum]                 INT                NOT NULL,
    [AccountGuid]                  UNIQUEIDENTIFIER   NOT NULL,
    [AccountName]                  NVARCHAR (MAX)     NOT NULL,
    [AccountAddress1]              NVARCHAR (MAX)     NULL,
    [AccountAddress2]              NVARCHAR (MAX)     NULL,
    [AccountCity]                  NVARCHAR (MAX)     NULL,
    [AccountState]                 NVARCHAR (MAX)     NOT NULL,
    [AccountZip]                   NVARCHAR (MAX)     NULL,
    [AccountPhone]                 NVARCHAR (MAX)     NULL,
    [AccountFax]                   NVARCHAR (MAX)     NULL,
    [AccountDiscountPercentage]    INT                NOT NULL,
    [AccountRepUser]               NVARCHAR (MAX)     NULL,
    [ShopGuid]                     UNIQUEIDENTIFIER   NOT NULL,
    [ShopName]                     NVARCHAR (MAX)     NOT NULL,
    [ShopNumber]                   INT                NULL,
    [ShopPhone]                    NVARCHAR (MAX)     NULL,
    [ShopFax]                      NVARCHAR (MAX)     NULL,
    [ShopAddress1]                 NVARCHAR (MAX)     NULL,
    [ShopAddress2]                 NVARCHAR (MAX)     NULL,
    [ShopCity]                     NVARCHAR (MAX)     NULL,
    [ShopState]                    NVARCHAR (MAX)     NOT NULL,
    [ShopZip]                      NVARCHAR (MAX)     NULL,
    [ShopNotes]                    NVARCHAR (MAX)     NULL,
    [ShopDiscountPercentage]       INT                NOT NULL,
    [ShopBillingCycleId]           INT                NULL,
    [ShopBillingCycleName]         NVARCHAR (MAX)     NULL,
    [ShopCCCId]                    NVARCHAR (128)     NULL,
    [ShopSelfScan]                 BIT                NOT NULL,
    [ShopRepUser]                  NVARCHAR (MAX)     NULL,
    [ShopAutomaticInvoicesInd]     BIT                NOT NULL,
    [RepairOrderId]                INT                NOT NULL,
    [RepairStatusId]               INT                NOT NULL,
    [RepairStatus]                 VARCHAR (9)        NULL,
    [RepairRONumber]               NVARCHAR (500)     NULL,
    [RepairInsuranceCompany]       NVARCHAR (200)     NULL,
    [RepairInsuranceClaimNumber]   NVARCHAR (200)     NULL,
    [RepairVehicleVIN]             NVARCHAR (128)     NULL,
    [RepairVehicleMake]            NVARCHAR (100)     NOT NULL,
    [RepairVehicleModel]           NVARCHAR (300)     NOT NULL,
    [RepairVehicleYear]            NVARCHAR (MAX)     NOT NULL,
    [RepairVehicleTransmission]    NVARCHAR (200)     NOT NULL,
    [RepairVehicleMakeType]        NVARCHAR (MAX)     NULL,
    [RepairVehicleFound]           BIT                NOT NULL,
    [RepairVehicleOdometer]        INT                NOT NULL,
    [RepairVehicleAirBagsDeployed] BIT                NOT NULL,
    [RepairVehicleDrivableInd]     BIT                NOT NULL,
    [RepairCreateByCCC]            INT                NOT NULL,
    [RepairCreatedByUser]          NVARCHAR (MAX)     NULL,
    [RepairCreatedDt]              DATETIMEOFFSET (7) NOT NULL,
    [RepairLastUpdatedByUser]      NVARCHAR (MAX)     NULL,
    [RepairLastUpdatedDt]          DATETIMEOFFSET (7) NULL,
    [RequestId]                    INT                NULL,
    [RequestTypeId]                INT                NULL,
    [RequestType]                  NVARCHAR (100)     NULL,
    [RequestTypeCategory]          NVARCHAR (MAX)     NULL,
    [RequestOtherWarningInfo]      NVARCHAR (MAX)     NULL,
    [RequestSeatRemovedInd]        BIT                NULL,
    [RequestProblemDescription]    NVARCHAR (MAX)     NULL,
    [RequestNotes]                 NVARCHAR (MAX)     NULL,
    [RequestCreatedByUser]         NVARCHAR (MAX)     NULL,
    [RequestCreatedDt]             DATETIMEOFFSET (7) NULL,
    [RequestLastUpdatedByUser]     NVARCHAR (MAX)     NULL,
    [RequestUpdatedDt]             DATETIMEOFFSET (7) NULL,
    [ReportId]                     INT                NULL,
    [ReportCreatedByUser]          NVARCHAR (MAX)     NULL,
    [ReportCreatedDt]              DATETIMEOFFSET (7) NULL,
    [ReportUpdatedByUser]          NVARCHAR (MAX)     NULL,
    [ReportUpdatedDt]              DATETIMEOFFSET (7) NULL,
    [ReportTechUser]               NVARCHAR (MAX)     NULL,
    [ReportTechUserProfileInd]     BIT                NULL,
    [ReportTechAssignedDt]         DATETIMEOFFSET (7) NULL,
    [ReportCompletedByUser]        NVARCHAR (MAX)     NULL,
    [ReportCompletedDt]            DATETIMEOFFSET (7) NULL,
    [ReportCancelled]              BIT                NULL,
    [ReportCancelReasonTypeId]     INT                NULL,
    [ReportCancelReasonTypeName]   NVARCHAR (MAX)     NULL,
    [ReportInvoicedInd]            BIT                NULL,
    [ReportInvoicedByUser]         NVARCHAR (MAX)     NULL,
    [ReportInvoicedDt]             DATETIMEOFFSET (7) NULL,
    [ReportInvoicedAmount]         DECIMAL (18, 2)    NULL,
    [ReportInvoiceDiscountAmount]  DECIMAL (18, 3)    NULL,
    [RepairInvoiceId]              INT                NULL,
    [RepairInvoiceMemo]            NVARCHAR (MAX)     NULL,
    [RepairInvoiceAmountApplied]   DECIMAL (18, 2)    NULL,
    [RepairInvoiceDiscountAmount]  DECIMAL (18, 2)    NULL,
    [RepairInvoiceCreatedByUser]   NVARCHAR (MAX)     NULL,
    [RepairInvoiceCreatedDt]       DATETIMEOFFSET (7) NULL,
    [PaymentID]                    INT                NULL,
    [PaymentType]                  NVARCHAR (MAX)     NULL,
    [PaymentDiscountPercentage]    INT                NULL,
    [PaymentTotalAmount]           DECIMAL (18, 2)    NULL,
    [PaymentRefNumber]             NVARCHAR (MAX)     NULL,
    [PaymentMemo]                  NVARCHAR (MAX)     NULL,
    [PaymentCreatedByUser]         NVARCHAR (MAX)     NULL,
    [PaymentCreatedDt]             DATETIMEOFFSET (7) NULL,
    [PaymentUpdatedByUser]         NVARCHAR (MAX)     NULL,
    [PaymentUpdatedDt]             DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_ReportData] PRIMARY KEY CLUSTERED ([DataGuid] ASC),
    CONSTRAINT [FK_DataLoadId] FOREIGN KEY ([DataLoadId]) REFERENCES [Reporting].[ReportDataLoads] ([ReportDataLoadId])
);








GO
CREATE NONCLUSTERED INDEX [IX_ReportDumpSearch]
    ON [Reporting].[ReportData]([ShopGuid] ASC, [AccountGuid] ASC, [RequestTypeId] ASC, [RepairStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DataChecksum]
    ON [Reporting].[ReportData]([DataChecksum] ASC);

