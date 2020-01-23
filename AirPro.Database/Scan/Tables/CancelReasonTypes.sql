CREATE TABLE [Scan].[CancelReasonTypes] (
    [CancelReasonTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (MAX) NULL,
    [Order]                INT            NOT NULL,
    [NotificationTemplate] INT            NULL,
    CONSTRAINT [PK_Scan.CancelReasonTypes] PRIMARY KEY CLUSTERED ([CancelReasonTypeId] ASC)
);

