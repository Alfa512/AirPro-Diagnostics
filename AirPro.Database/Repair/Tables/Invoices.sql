CREATE TABLE [Repair].[Invoices] (
    [InvoiceId]          INT                NOT NULL,
    [CustomerMemo]       NVARCHAR (MAX)     NULL,
    [CreatedDt]          DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDt]          DATETIMEOFFSET (7) NULL,
    [CreatedByUserGuid]  UNIQUEIDENTIFIER   NOT NULL,
    [UpdatedByUserGuid]  UNIQUEIDENTIFIER   NULL,
    [InvoicedInd]        BIT                DEFAULT ((0)) NOT NULL,
    [InvoicedDt]         DATETIMEOFFSET (7) NULL,
    [InvoicedByUserGuid] UNIQUEIDENTIFIER   NULL,
    [CurrencyId]         INT                NULL,
    CONSTRAINT [PK_Repair.Invoices] PRIMARY KEY CLUSTERED ([InvoiceId] ASC),
    CONSTRAINT [FK_Repair.Invoices_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Repair.Invoices_Access.Users_InvoicedByUserGuid] FOREIGN KEY ([InvoicedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Repair.Invoices_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Repair.Invoices_Billing.Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [Billing].[Currencies] ([CurrencyId]),
    CONSTRAINT [FK_Repair.Invoices_Repair.Orders_InvoiceID] FOREIGN KEY ([InvoiceId]) REFERENCES [Repair].[Orders] ([OrderId])
);










GO



GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Repair].[Invoices]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvoicedByUserGuid]
    ON [Repair].[Invoices]([InvoicedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Repair].[Invoices]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CurrencyId]
    ON [Repair].[Invoices]([CurrencyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RepairInvoices_InvoiceInd_CurrencyId_InvoicedDt]
    ON [Repair].[Invoices]([InvoicedInd] ASC)
    INCLUDE([CurrencyId], [InvoicedDt]);

