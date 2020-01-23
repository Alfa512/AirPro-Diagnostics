CREATE TABLE [Billing].[PaymentTypes] (
    [PaymentTypeID]        INT            IDENTITY (1, 1) NOT NULL,
    [PaymentTypeName]      NVARCHAR (MAX) NULL,
    [PaymentTypeSortOrder] INT            NOT NULL,
    [PaymentTypeActiveInd] BIT            NOT NULL,
    CONSTRAINT [PK_Billing.PaymentTypes] PRIMARY KEY CLUSTERED ([PaymentTypeID] ASC)
);

