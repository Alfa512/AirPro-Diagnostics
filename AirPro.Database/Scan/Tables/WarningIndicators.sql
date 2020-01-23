CREATE TABLE [Scan].[WarningIndicators] (
    [WarningIndicatorID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Scan.WarningIndicators] PRIMARY KEY CLUSTERED ([WarningIndicatorID] ASC)
);

