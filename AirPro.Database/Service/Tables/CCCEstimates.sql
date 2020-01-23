CREATE TABLE [Service].[CCCEstimates] (
    [EstimateId]           INT                IDENTITY (1, 1) NOT NULL,
    [RequestGuid]          UNIQUEIDENTIFIER   NULL,
    [AppId]                INT                NOT NULL,
    [Trigger]              NVARCHAR (MAX)     NULL,
    [DocumentGuid]         UNIQUEIDENTIFIER   NULL,
    [DocumentVersion]      INT                NULL,
    [DocumentStatus]       NVARCHAR (MAX)     NULL,
    [ShopId]               NVARCHAR (128)     NULL,
    [ShopName]             NVARCHAR (MAX)     NULL,
    [ShopRoNumber]         NVARCHAR (MAX)     NULL,
    [VehicleVin]           NVARCHAR (128)     NULL,
    [VehicleYear]          NVARCHAR (MAX)     NULL,
    [VehicleMake]          NVARCHAR (MAX)     NULL,
    [VehicleModel]         NVARCHAR (MAX)     NULL,
    [VehicleOdometer]      NVARCHAR (MAX)     NULL,
    [VehicleDrivable]      BIT                NULL,
    [InsuranceCompanyId]   NVARCHAR (128)     NULL,
    [InsuranceCompanyName] NVARCHAR (MAX)     NULL,
    [RawXml]               XML                NOT NULL,
    [ReceivedDt]           DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [ProcessedInd]         BIT                DEFAULT ((0)) NOT NULL,
    [ProcessedDt]          DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NOT NULL,
    CONSTRAINT [PK_Service.CCCEstimates] PRIMARY KEY CLUSTERED ([EstimateId] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_InsuranceCompanyId]
    ON [Service].[CCCEstimates]([InsuranceCompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleVin]
    ON [Service].[CCCEstimates]([VehicleVin] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopId]
    ON [Service].[CCCEstimates]([ShopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DocumentGuid]
    ON [Service].[CCCEstimates]([DocumentGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestGuid]
    ON [Service].[CCCEstimates]([RequestGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_ServiceCCCEstimates_ProcessedInd]
    ON [Service].[CCCEstimates]([ProcessedInd] ASC)
    INCLUDE([EstimateId], [DocumentGuid]);


GO
CREATE NONCLUSTERED INDEX [IX_ServiceCCCEstimates_InsuranceCompanyId]
    ON [Service].[CCCEstimates]([InsuranceCompanyId] ASC)
    INCLUDE([InsuranceCompanyName]);

