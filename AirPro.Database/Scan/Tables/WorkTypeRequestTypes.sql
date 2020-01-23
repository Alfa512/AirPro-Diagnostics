CREATE TABLE [Scan].[WorkTypeRequestTypes] (
    [WorkTypeId]    INT NOT NULL,
    [RequestTypeId] INT NOT NULL,
    CONSTRAINT [PK_Scan.WorkTypeRequestTypes] PRIMARY KEY CLUSTERED ([WorkTypeId] ASC, [RequestTypeId] ASC),
    CONSTRAINT [FK_Scan.WorkTypeRequestTypes_Scan.RequestTypes_RequestTypeId] FOREIGN KEY ([RequestTypeId]) REFERENCES [Scan].[RequestTypes] ([RequestTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Scan.WorkTypeRequestTypes_Scan.WorkTypes_WorkTypeId] FOREIGN KEY ([WorkTypeId]) REFERENCES [Scan].[WorkTypes] ([WorkTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RequestTypeId]
    ON [Scan].[WorkTypeRequestTypes]([RequestTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkTypeId]
    ON [Scan].[WorkTypeRequestTypes]([WorkTypeId] ASC);

