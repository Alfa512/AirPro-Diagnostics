CREATE TABLE [Repair].[OrderStatuses] (
    [StatusId]   INT           IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Repair.OrderStatuses] PRIMARY KEY CLUSTERED ([StatusId] ASC)
);

