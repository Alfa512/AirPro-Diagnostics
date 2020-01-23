CREATE TABLE [Access].[Roles] (
    [RoleGuid] UNIQUEIDENTIFIER NOT NULL,
    [Name]     NVARCHAR (256)   NOT NULL,
    CONSTRAINT [PK_Access.Roles] PRIMARY KEY CLUSTERED ([RoleGuid] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [Access].[Roles]([Name] ASC);

