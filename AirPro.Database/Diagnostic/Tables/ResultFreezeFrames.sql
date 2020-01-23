CREATE TABLE [Diagnostic].[ResultFreezeFrames] (
    [FreezeFrameId]          INT            IDENTITY (1, 1) NOT NULL,
    [ResultId]               INT            NOT NULL,
    [ControllerId]           INT            NOT NULL,
    [FreezeFrameTroubleCode] NVARCHAR (100) NULL,
    [SensorGroupsJson]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Diagnostic.ResultFreezeFrames] PRIMARY KEY CLUSTERED ([FreezeFrameId] ASC),
    CONSTRAINT [FK_Diagnostic.ResultFreezeFrames_Diagnostic.Controllers_ControllerId] FOREIGN KEY ([ControllerId]) REFERENCES [Diagnostic].[Controllers] ([ControllerId]),
    CONSTRAINT [FK_Diagnostic.ResultFreezeFrames_Diagnostic.Results_ResultId] FOREIGN KEY ([ResultId]) REFERENCES [Diagnostic].[Results] ([ResultId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ControllerId]
    ON [Diagnostic].[ResultFreezeFrames]([ControllerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ResultId]
    ON [Diagnostic].[ResultFreezeFrames]([ResultId] ASC);

