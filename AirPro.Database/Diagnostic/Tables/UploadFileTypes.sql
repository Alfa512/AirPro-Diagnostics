CREATE TABLE [Diagnostic].[UploadFileTypes] (
    [UploadFileTypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [UploadFileTypeName] NVARCHAR (20) NULL,
    CONSTRAINT [PK_Diagnostic.UploadFileTypes] PRIMARY KEY CLUSTERED ([UploadFileTypeId] ASC)
);

