CREATE TABLE [Service].[MitchellRequests] (
    [RequestId]          INT                IDENTITY (1, 1) NOT NULL,
    [ShopGuid]           UNIQUEIDENTIFIER   NULL,
    [MitchellRecId]      NVARCHAR (128)     NULL,
    [VehicleVIN]         NVARCHAR (128)     NULL,
    [ShopRONum]          NVARCHAR (128)     NULL,
    [InsuranceCoName]    NVARCHAR (128)     NULL,
    [Odometer]           INT                NULL,
    [DrivableInd]        BIT                NOT NULL,
    [AirBagsDeployedInd] BIT                NOT NULL,
    [RequestBody]        NVARCHAR (MAX)     NULL,
    [RequestDt]          DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_Service.MitchellRequests] PRIMARY KEY CLUSTERED ([RequestId] ASC),
    CONSTRAINT [FK_Service.MitchellRequests_Access.Shops_ShopGuid] FOREIGN KEY ([ShopGuid]) REFERENCES [Access].[Shops] ([ShopGuid])
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleVIN]
    ON [Service].[MitchellRequests]([VehicleVIN] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MitchellRecId]
    ON [Service].[MitchellRequests]([MitchellRecId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopGuid]
    ON [Service].[MitchellRequests]([ShopGuid] ASC);

