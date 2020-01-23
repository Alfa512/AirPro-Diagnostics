CREATE TABLE [Access].[ShopContacts] (
    [ShopContactGuid] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [FirstName]       NVARCHAR (MAX)   NULL,
    [LastName]        NVARCHAR (MAX)   NULL,
    [PhoneNumber]     NVARCHAR (MAX)   NULL,
    [ShopGuid]        UNIQUEIDENTIFIER NOT NULL,
    [DeletedInd]      BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Access.ShopContacts] PRIMARY KEY CLUSTERED ([ShopContactGuid] ASC),
    CONSTRAINT [FK_Access.ShopContacts_Access.Shops_ShopGuid] FOREIGN KEY ([ShopGuid]) REFERENCES [Access].[Shops] ([ShopGuid]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ShopGuid]
    ON [Access].[ShopContacts]([ShopGuid] ASC);

