CREATE TABLE [Common].[UploadTypes] (
    [UploadTypeId]     INT           IDENTITY (1, 1) NOT NULL,
    [UploadTypeName]   NVARCHAR (50) NULL,
    [UploadTypeSchema] NVARCHAR (50) NULL,
    [UploadTypeTable]  NVARCHAR (50) NULL,
    CONSTRAINT [PK_Common.UploadTypes] PRIMARY KEY CLUSTERED ([UploadTypeId] ASC)
);

