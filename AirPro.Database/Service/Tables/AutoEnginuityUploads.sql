CREATE TABLE [Service].[AutoEnginuityUploads] (
    [UploadId]     INT                IDENTITY (1, 1) NOT NULL,
    [RequestQuery] NVARCHAR (MAX)     NULL,
    [RequestBody]  NVARCHAR (MAX)     NULL,
    [RequestDt]    DATETIMEOFFSET (7) DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([UploadId] ASC)
);

