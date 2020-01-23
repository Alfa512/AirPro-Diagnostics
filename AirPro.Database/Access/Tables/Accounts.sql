CREATE TABLE [Access].[Accounts] (
    [AccountGuid]        UNIQUEIDENTIFIER   CONSTRAINT [DF_Access.Accounts_AccountGuid] DEFAULT (newsequentialid()) NOT NULL,
    [Name]               NVARCHAR (MAX)     NOT NULL,
    [CreatedByUserGuid]  UNIQUEIDENTIFIER   NOT NULL,
    [CreatedDt]          DATETIMEOFFSET (7) NOT NULL,
    [UpdatedByUserGuid]  UNIQUEIDENTIFIER   NULL,
    [UpdatedDt]          DATETIMEOFFSET (7) NULL,
    [Address1]           NVARCHAR (MAX)     NULL,
    [Address2]           NVARCHAR (MAX)     NULL,
    [City]               NVARCHAR (MAX)     NULL,
    [StateId]            INT                DEFAULT ((0)) NOT NULL,
    [Zip]                NVARCHAR (MAX)     NULL,
    [Phone]              NVARCHAR (MAX)     NULL,
    [Fax]                NVARCHAR (MAX)     NULL,
    [DiscountPercentage] INT                DEFAULT ((0)) NOT NULL,
    [ActiveInd]          BIT                DEFAULT ((1)) NOT NULL,
    [EmployeeGuid]       UNIQUEIDENTIFIER   NULL,
    CONSTRAINT [PK_Access.Accounts] PRIMARY KEY CLUSTERED ([AccountGuid] ASC),
    CONSTRAINT [FK_Access.Accounts_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Accounts_Access.Users_EmployeeGuid] FOREIGN KEY ([EmployeeGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Accounts_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Access.Accounts_Common.States_StateId] FOREIGN KEY ([StateId]) REFERENCES [Common].[States] ([StateId])
);








GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Access].[Accounts]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Access].[Accounts]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StateId]
    ON [Access].[Accounts]([StateId] ASC);


GO
CREATE TRIGGER [Access].[trgAccessAccountsArchive] ON [Access].[Accounts]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.AccountsArchive
	(
		AccountGuid
		,Name
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,Address1
		,Address2
		,City
		,StateId
		,Zip
		,Phone
		,Fax
		,DiscountPercentage
		,ActiveInd
		,EmployeeGuid
	)
	SELECT
		AccountGuid
		,Name
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,Address1
		,Address2
		,City
		,StateId
		,Zip
		,Phone
		,Fax
		,DiscountPercentage
		,ActiveInd
		,EmployeeGuid
	FROM deleted

END
GO
CREATE NONCLUSTERED INDEX [IX_EmployeeGuid]
    ON [Access].[Accounts]([EmployeeGuid] ASC);

