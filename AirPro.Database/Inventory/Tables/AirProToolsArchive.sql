CREATE TABLE [Inventory].[AirProToolsArchive] (
    [ToolArchiveId]          INT                IDENTITY (1, 1) NOT NULL,
    [ToolId]                 INT                NOT NULL,
    [ToolPassword]           NVARCHAR (20)      NULL,
    [AutoEnginuityNum]       NVARCHAR (20)      NULL,
    [AutoEnginuityVersion]   NVARCHAR (20)      NULL,
    [CarDaqNum]              NVARCHAR (20)      NULL,
    [DGNum]                  NVARCHAR (20)      NULL,
    [TeamViewerId]           NVARCHAR (20)      NULL,
    [TeamViewerPassword]     NVARCHAR (20)      NULL,
    [WindowsVersion]         NVARCHAR (100)     NULL,
    [TabletModel]            NVARCHAR (100)     NULL,
    [HubModel]               NVARCHAR (200)     NULL,
    [IPV6DisabledInd]        BIT                NOT NULL,
    [OneDriveSyncEnabledInd] BIT                NOT NULL,
    [UpdatesServiceInd]      BIT                NOT NULL,
    [MeteredConnectionInd]   BIT                NOT NULL,
    [SelfScanEnabledInd]     BIT                NOT NULL,
    [CreatedByUserGuid]      UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]              DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]      UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]              DATETIMEOFFSET (7) NULL,
    [ToolKey]                UNIQUEIDENTIFIER   DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [OBD2YConnector]         NVARCHAR (100)     NULL,
    [AELatestCode]           NVARCHAR (100)     NULL,
    [ChargerStyle]           NVARCHAR (100)     NULL,
    [TabletSerialNumber]     NVARCHAR (100)     NULL,
    [WifiCard]               NVARCHAR (100)     NULL,
    [WifiHardwareId]         NVARCHAR (100)     NULL,
    [WifiDriverDate]         DATETIMEOFFSET (7) NULL,
    [WifiDriverVersion]      NVARCHAR (100)     NULL,
    [ImageVersion]           NVARCHAR (100)     NULL,
    [HondaVersion]           NVARCHAR (100)     NULL,
    [FJDSVersion]            NVARCHAR (100)     NULL,
    [TechstreamVersion]      NVARCHAR (100)     NULL,
    [CellularActiveInd]      BIT                DEFAULT ((0)) NOT NULL,
    [CellularProvider]       NVARCHAR (100)     NULL,
    [CellularIMEI]           NVARCHAR (100)     NULL,
    [WifiMacAddress]         NVARCHAR (100)     NULL,
    [J2534Brand]             INT                NULL,
    [J2534Model]             INT                NULL,
    [Type]                   INT                DEFAULT ((0)) NOT NULL,
    [J2534Serial]            NVARCHAR (100)     NULL,
    CONSTRAINT [PK_Inventory.AirProToolsArchive] PRIMARY KEY CLUSTERED ([ToolArchiveId] ASC),
    CONSTRAINT [FK_Inventory.AirProToolsArchive_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Inventory.AirProToolsArchive_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Inventory].[AirProToolsArchive]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Inventory].[AirProToolsArchive]([CreatedByUserGuid] ASC);


GO



GO
CREATE NONCLUSTERED INDEX [IX_ToolId]
    ON [Inventory].[AirProToolsArchive]([ToolId] ASC);

