CREATE TABLE [Diagnostic].[Uploads] (
    [UploadId]         INT            IDENTITY (1, 1) NOT NULL,
    [ResultId]         INT            NULL,
    [UploadFileTypeId] INT            NOT NULL,
    [UploadText]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Diagnostic.Uploads] PRIMARY KEY CLUSTERED ([UploadId] ASC),
    CONSTRAINT [FK_Diagnostic.Uploads_Diagnostic.Results_ResultId] FOREIGN KEY ([ResultId]) REFERENCES [Diagnostic].[Results] ([ResultId]),
    CONSTRAINT [FK_Diagnostic.Uploads_Diagnostic.UploadFileTypes_UploadFileTypeId] FOREIGN KEY ([UploadFileTypeId]) REFERENCES [Diagnostic].[UploadFileTypes] ([UploadFileTypeId])
);




GO
CREATE NONCLUSTERED INDEX [IX_UploadFileTypeId]
    ON [Diagnostic].[Uploads]([UploadFileTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ResultId]
    ON [Diagnostic].[Uploads]([ResultId] ASC);

