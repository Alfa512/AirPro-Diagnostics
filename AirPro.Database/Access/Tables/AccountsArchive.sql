CREATE TABLE [Access].[AccountsArchive] (
    [ArchiveId]          INT                IDENTITY (1, 1) NOT NULL,
    [ArchiveDt]          DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [AccountGuid]        UNIQUEIDENTIFIER   NOT NULL,
    [ActiveInd]          BIT                NOT NULL,
    [Address1]           NVARCHAR (MAX)     NULL,
    [Address2]           NVARCHAR (MAX)     NULL,
    [City]               NVARCHAR (MAX)     NULL,
    [DiscountPercentage] INT                NOT NULL,
    [Fax]                NVARCHAR (MAX)     NULL,
    [Name]               NVARCHAR (MAX)     NULL,
    [Phone]              NVARCHAR (MAX)     NULL,
    [StateId]            INT                NOT NULL,
    [Zip]                NVARCHAR (MAX)     NULL,
    [CreatedByUserGuid]  UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]          DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]  UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]          DATETIMEOFFSET (7) NULL,
    [EmployeeGuid]       UNIQUEIDENTIFIER   NULL,
    CONSTRAINT [PK_Access.AccountsArchive] PRIMARY KEY CLUSTERED ([ArchiveId] ASC)
);



