CREATE TABLE [Repair].[Orders] (
    [OrderId]                  INT                IDENTITY (1, 1) NOT NULL,
    [Status]                   INT                NOT NULL,
    [ShopReferenceNumber]      NVARCHAR (500)     NULL,
    [InsuranceCompanyOther]    NVARCHAR (200)     NULL,
    [InsuranceReferenceNumber] NVARCHAR (200)     NULL,
    [Odometer]                 INT                NOT NULL,
    [AirBagsDeployed]          BIT                NOT NULL,
    [VehicleVIN]               NVARCHAR (128)     NULL,
    [CreatedDt]                DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NOT NULL,
    [UpdatedDt]                DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NULL,
    [ShopGuid]                 UNIQUEIDENTIFIER   NOT NULL,
    [CreatedByUserGuid]        UNIQUEIDENTIFIER   NOT NULL,
    [UpdatedByUserGuid]        UNIQUEIDENTIFIER   NULL,
    [InsuranceCompanyId]       INT                NOT NULL,
    [DrivableInd]              BIT                DEFAULT ((0)) NOT NULL,
    [CCCDocumentGuid]          UNIQUEIDENTIFIER   NULL,
    [MitchellRequestId]        INT                NULL,
    [AirBagsVisualDeployments] NVARCHAR (1000)    NULL,
    CONSTRAINT [PK_Repair.Orders] PRIMARY KEY CLUSTERED ([OrderId] ASC),
    CONSTRAINT [FK_Repair.Orders_Access.Shops_ShopGuid] FOREIGN KEY ([ShopGuid]) REFERENCES [Access].[Shops] ([ShopGuid]),
    CONSTRAINT [FK_Repair.Orders_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Repair.Orders_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Repair.Orders_Repair.InsuranceCompanies_InsuranceCompanyID] FOREIGN KEY ([InsuranceCompanyId]) REFERENCES [Repair].[InsuranceCompanies] ([InsuranceCompanyId]),
    CONSTRAINT [FK_Repair.Orders_Repair.Vehicles_Vehicle_VIN] FOREIGN KEY ([VehicleVIN]) REFERENCES [Repair].[Vehicles] ([VehicleVIN]),
    CONSTRAINT [FK_Repair.Orders_Service.MitchellRequests_MitchellRequestId] FOREIGN KEY ([MitchellRequestId]) REFERENCES [Service].[MitchellRequests] ([RequestId])
);












GO



GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_Vehicle_VIN]
    ON [Repair].[Orders]([VehicleVIN] ASC);




GO
CREATE NONCLUSTERED INDEX [IX_InsuranceCompanyID]
    ON [Repair].[Orders]([InsuranceCompanyID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Repair].[Orders]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopGuid]
    ON [Repair].[Orders]([ShopGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Repair].[Orders]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CCCDocumentGuid]
    ON [Repair].[Orders]([CCCDocumentGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_OrderId_ShopGuid_VehicleVIN]
    ON [Repair].[Orders]([OrderId] ASC)
    INCLUDE([ShopGuid], [VehicleVIN]);


GO
CREATE NONCLUSTERED INDEX [IX_MitchellRequestId]
    ON [Repair].[Orders]([MitchellRequestId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RepairOrders_ShopGuid_Status]
    ON [Repair].[Orders]([ShopGuid] ASC, [Status] ASC)
    INCLUDE([AirBagsDeployed], [CCCDocumentGuid], [CreatedByUserGuid], [CreatedDt], [DrivableInd], [InsuranceCompanyId], [InsuranceCompanyOther], [InsuranceReferenceNumber], [Odometer], [ShopReferenceNumber], [UpdatedByUserGuid], [UpdatedDt], [VehicleVIN]);

