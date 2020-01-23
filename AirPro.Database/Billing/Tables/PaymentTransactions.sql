CREATE TABLE [Billing].[PaymentTransactions] (
    [PaymentTransactionId]             INT                IDENTITY (1, 1) NOT NULL,
    [PaymentId]                        INT                NOT NULL,
    [InvoiceId]                        INT                NOT NULL,
    [InvoiceAmountApplied]             DECIMAL (18, 2)    NOT NULL,
    [PaymentTransactionVoidInd]        BIT                NOT NULL,
    [PaymentTransactionVoidByUserGuid] UNIQUEIDENTIFIER   NULL,
    [PaymentTransactionVoidDt]         DATETIMEOFFSET (7) NULL,
    [CreatedDt]                        DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDt]                        DATETIMEOFFSET (7) NULL,
    [CreatedByUserGuid]                UNIQUEIDENTIFIER   NOT NULL,
    [UpdatedByUserGuid]                UNIQUEIDENTIFIER   NULL,
    [DiscountAmountApplied]            DECIMAL (18, 2)    DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Billing.PaymentTransactions] PRIMARY KEY CLUSTERED ([PaymentTransactionId] ASC),
    CONSTRAINT [FK_Billing.PaymentTransactions_Access.Users_CreatedByUserGuid] FOREIGN KEY ([CreatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Billing.PaymentTransactions_Access.Users_PaymentTransactionVoidByUserGuid] FOREIGN KEY ([PaymentTransactionVoidByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Billing.PaymentTransactions_Access.Users_UpdatedByUserGuid] FOREIGN KEY ([UpdatedByUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Billing.PaymentTransactions_Billing.Payments_PaymentID] FOREIGN KEY ([PaymentId]) REFERENCES [Billing].[Payments] ([PaymentID]),
    CONSTRAINT [FK_Billing.PaymentTransactions_Repair.Invoices_InvoiceID] FOREIGN KEY ([InvoiceId]) REFERENCES [Repair].[Invoices] ([InvoiceId])
);






GO



GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_InvoiceID]
    ON [Billing].[PaymentTransactions]([InvoiceID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PaymentID]
    ON [Billing].[PaymentTransactions]([PaymentID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UpdatedByUserGuid]
    ON [Billing].[PaymentTransactions]([UpdatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PaymentTransactionVoidByUserGuid]
    ON [Billing].[PaymentTransactions]([PaymentTransactionVoidByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedByUserGuid]
    ON [Billing].[PaymentTransactions]([CreatedByUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PaymentTransactions_PaymentId_InvoiceId]
    ON [Billing].[PaymentTransactions]([PaymentId] ASC)
    INCLUDE([InvoiceId]);

