CREATE TABLE [Diagnostic].[Results] (
    [ResultId]          INT                IDENTITY (1, 1) NOT NULL,
    [DiagnosticToolId]  INT                NOT NULL,
    [RequestId]         INT                NULL,
    [ScanDateTime]      DATETIME           NULL,
    [CustomerFirstName] NVARCHAR (50)      NULL,
    [CustomerLastName]  NVARCHAR (50)      NULL,
    [CustomerRo]        NVARCHAR (50)      NULL,
    [ShopName]          NVARCHAR (150)     NULL,
    [ShopAddress]       NVARCHAR (150)     NULL,
    [ShopEmail]         NVARCHAR (150)     NULL,
    [ShopFax]           NVARCHAR (50)      NULL,
    [ShopPhone]         NVARCHAR (50)      NULL,
    [VehicleVin]        NVARCHAR (50)      NULL,
    [VehicleMake]       NVARCHAR (50)      NULL,
    [VehicleModel]      NVARCHAR (100)     NULL,
    [VehicleYear]       NVARCHAR (10)      NULL,
    [TestabilityIssues] NVARCHAR (MAX)     NULL,
    [DeletedInd]        BIT                NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Diagnostic.Results] PRIMARY KEY CLUSTERED ([ResultId] ASC),
    CONSTRAINT [FK_Diagnostic.Results_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Diagnostic.Results_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Diagnostic.Results_Diagnostic.Tools_DiagnosticToolId] FOREIGN KEY ([DiagnosticToolId]) REFERENCES [Diagnostic].[Tools] ([DiagnosticToolId]),
    CONSTRAINT [FK_Diagnostic.Results_Scan.Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Scan].[Requests] ([RequestId])
);




GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Diagnostic].[Results]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Diagnostic].[Results]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleVin]
    ON [Diagnostic].[Results]([VehicleVin] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestId]
    ON [Diagnostic].[Results]([RequestId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DiagnosticToolId]
    ON [Diagnostic].[Results]([DiagnosticToolId] ASC);

