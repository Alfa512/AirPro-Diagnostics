CREATE TABLE [Billing].[Cycles] (
    [CycleId]   INT            IDENTITY (1, 1) NOT NULL,
    [CycleName] NVARCHAR (MAX) NULL,
    [SortOrder] INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Billing.Cycles] PRIMARY KEY CLUSTERED ([CycleId] ASC)
);



