CREATE TABLE [Billing].[EstimatePlans] (
    [EstimatePlanId]    INT                IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (MAX)     NULL,
    [Description]       NVARCHAR (MAX)     NULL,
    [ActiveInd]         BIT                NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Billing.EstimatePlans] PRIMARY KEY CLUSTERED ([EstimatePlanId] ASC),
    CONSTRAINT [FK_Billing.EstimatePlans_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Billing.EstimatePlans_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Billing].[EstimatePlans]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Billing].[EstimatePlans]([CreatedByUserGuid] ASC);

