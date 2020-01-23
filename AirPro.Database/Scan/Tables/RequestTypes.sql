CREATE TABLE [Scan].[RequestTypes] (
    [RequestTypeId]     INT                IDENTITY (1, 1) NOT NULL,
    [TypeName]          NVARCHAR (100)     NULL,
    [ActiveFlag]        BIT                NOT NULL,
    [BillableFlag]      BIT                NOT NULL,
    [SortOrder]         INT                DEFAULT ((0)) NOT NULL,
    [CreatedByUserGuid] UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]         DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]         DATETIMEOFFSET (7) NULL,
    [DefaultPrice]      FLOAT (53)         DEFAULT ((0)) NOT NULL,
    [InvoiceMemo]       NVARCHAR (800)     NULL,
    [Instructions]      NVARCHAR (800)     NULL,
    CONSTRAINT [PK_Scan.RequestTypes] PRIMARY KEY CLUSTERED ([RequestTypeId] ASC),
    CONSTRAINT [FK_Scan.RequestTypes_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Scan.RequestTypes_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid])
);












GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Scan].[RequestTypes]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId]
    ON [Scan].[RequestTypes]([RequestTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Scan].[RequestTypes]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId_TypeName]
    ON [Scan].[RequestTypes]([RequestTypeId] ASC)
    INCLUDE([TypeName]);

