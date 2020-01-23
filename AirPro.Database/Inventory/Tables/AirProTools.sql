CREATE TABLE [Inventory].[AirProTools] (
    [ToolId]                 INT                IDENTITY (1, 1) NOT NULL,
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
    [ToolKey]                UNIQUEIDENTIFIER   DEFAULT (newid()) NOT NULL,
    [J2534Brand]             INT                NULL,
    [J2534Model]             INT                NULL,
    [J2534Serial]            NVARCHAR (100)     NULL,
    [Type]                   INT                DEFAULT ((1)) NOT NULL,
    [ToolName]               AS                 (case [Type] when (2) then 'FP' when (3) then 'EP' else 'AP' end+right('00000'+CONVERT([varchar](10),[ToolId]),(5))),
    CONSTRAINT [PK_Inventory.AirProTools] PRIMARY KEY CLUSTERED ([ToolId] ASC),
    CONSTRAINT [FK_Inventory.AirProTools_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Inventory.AirProTools_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);








GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Inventory].[AirProTools]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Inventory].[AirProTools]([CreatedByUserGuid] ASC);


GO



GO
CREATE TRIGGER [Inventory].[trgAirProToolArchive] ON [Inventory].[AirProTools] 
FOR UPDATE, DELETE
    AS
	    INSERT INTO Inventory.AirProToolsArchive
	    (
		    ToolId
		    ,ToolKey
		    ,ToolPassword
		    ,AutoEnginuityNum
		    ,AutoEnginuityVersion
		    ,CarDaqNum
		    ,DGNum
		    ,TeamViewerId
		    ,TeamViewerPassword
		    ,WindowsVersion
		    ,TabletModel
		    ,HubModel
		    ,IPV6DisabledInd
		    ,OneDriveSyncEnabledInd
		    ,UpdatesServiceInd
		    ,MeteredConnectionInd
		    ,SelfScanEnabledInd
		    ,CreatedByUserGuid
		    ,CreatedDt
		    ,UpdatedByUserGuid
		    ,UpdatedDt
		    ,OBD2YConnector
		    ,AELatestCode
		    ,ChargerStyle
		    ,TabletSerialNumber
		    ,WifiCard
		    ,WifiHardwareId
		    ,WifiDriverDate
		    ,WifiDriverVersion
		    ,ImageVersion
		    ,HondaVersion
		    ,FJDSVersion
		    ,TechstreamVersion
		    ,CellularActiveInd
		    ,CellularProvider
		    ,CellularIMEI
		    ,WifiMacAddress
		    ,J2534Brand
		    ,J2534Model
		    ,J2534Serial
		    ,Type
	    )
	    SELECT
		    ToolId
		    ,ToolKey
		    ,ToolPassword
		    ,AutoEnginuityNum
		    ,AutoEnginuityVersion
		    ,CarDaqNum
		    ,DGNum
		    ,TeamViewerId
		    ,TeamViewerPassword
		    ,WindowsVersion
		    ,TabletModel
		    ,HubModel
		    ,IPV6DisabledInd
		    ,OneDriveSyncEnabledInd
		    ,UpdatesServiceInd
		    ,MeteredConnectionInd
		    ,SelfScanEnabledInd
		    ,CreatedByUserGuid
		    ,CreatedDt
		    ,UpdatedByUserGuid
		    ,UpdatedDt
		    ,OBD2YConnector
		    ,AELatestCode
		    ,ChargerStyle
		    ,TabletSerialNumber
		    ,WifiCard
		    ,WifiHardwareId
		    ,WifiDriverDate
		    ,WifiDriverVersion
		    ,ImageVersion
		    ,HondaVersion
		    ,FJDSVersion
		    ,TechstreamVersion
		    ,CellularActiveInd
		    ,CellularProvider
		    ,CellularIMEI
		    ,WifiMacAddress
		    ,J2534Brand
		    ,J2534Model
		    ,J2534Serial
		    ,Type
	    FROM DELETED
GO
CREATE NONCLUSTERED INDEX [IX_ToolKey]
    ON [Inventory].[AirProTools]([ToolKey] ASC);

