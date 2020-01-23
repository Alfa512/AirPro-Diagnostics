CREATE TABLE [Access].[Registrations] (
    [RegistrationId]         UNIQUEIDENTIFIER   DEFAULT (newsequentialid()) NOT NULL,
    [RegistrationStatus]     INT                NOT NULL,
    [StatusUpdateDt]         DATETIMEOFFSET (7) NULL,
    [CallbackUrl]            NVARCHAR (MAX)     NULL,
    [Email]                  NVARCHAR (MAX)     NULL,
    [DifferentShopInfo]      BIT                NOT NULL,
    [CompletedDt]            DATETIMEOFFSET (7) NULL,
    [RegistrationUserId]     INT                NOT NULL,
    [RegistrationAccountId]  INT                NOT NULL,
    [RegistrationShopId]     INT                NOT NULL,
    [AccountGuid]            UNIQUEIDENTIFIER   NULL,
    [ShopGuid]               UNIQUEIDENTIFIER   NULL,
    [ClientUserGuid]         UNIQUEIDENTIFIER   NULL,
    [StatusUpdateByUserGuid] UNIQUEIDENTIFIER   NULL,
    [CompletedByUserGuid]    UNIQUEIDENTIFIER   NULL,
    [PassedStep]             INT                NULL,
    [CreatedByUserGuid]      UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]              DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]      UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]              DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Access.Registrations] PRIMARY KEY CLUSTERED ([RegistrationId] ASC),
    CONSTRAINT [FK_Access.Registrations_Access.Accounts_AccountGuid] FOREIGN KEY ([AccountGuid]) REFERENCES [Access].[Accounts] ([AccountGuid]),
    CONSTRAINT [FK_Access.Registrations_Access.RegistrationAccounts_RegistrationAccountId] FOREIGN KEY ([RegistrationAccountId]) REFERENCES [Access].[RegistrationAccounts] ([RegistrationAccountId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Access.Registrations_Access.RegistrationShops_RegistrationShopId] FOREIGN KEY ([RegistrationShopId]) REFERENCES [Access].[RegistrationShops] ([RegistrationShopId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Access.Registrations_Access.RegistrationUsers_RegistrationUserId] FOREIGN KEY ([RegistrationUserId]) REFERENCES [Access].[RegistrationUsers] ([RegistrationUserId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Access.Registrations_Access.Shops_ShopGuid] FOREIGN KEY ([ShopGuid]) REFERENCES [Access].[Shops] ([ShopGuid]),
    CONSTRAINT [FK_Access.Registrations_Access.Users_ClientUserGuid] FOREIGN KEY ([ClientUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Registrations_Access.Users_CompletedByUserGuid] FOREIGN KEY ([CompletedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Registrations_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Registrations_Access.Users_StatusUpdateByUserGuid] FOREIGN KEY ([StatusUpdateByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Registrations_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[Registrations]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[Registrations]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CompletedByUserGuid]
    ON [Access].[Registrations]([CompletedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StatusUpdateByUserGuid]
    ON [Access].[Registrations]([StatusUpdateByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ClientUserGuid]
    ON [Access].[Registrations]([ClientUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShopGuid]
    ON [Access].[Registrations]([ShopGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AccountGuid]
    ON [Access].[Registrations]([AccountGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RegistrationShopId]
    ON [Access].[Registrations]([RegistrationShopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RegistrationAccountId]
    ON [Access].[Registrations]([RegistrationAccountId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RegistrationUserId]
    ON [Access].[Registrations]([RegistrationUserId] ASC);

