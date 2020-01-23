CREATE TABLE [Support].[RequestLogExceptions] (
    [ExcptionId]          INT            IDENTITY (1, 1) NOT NULL,
    [ExceptionMessage]    NVARCHAR (MAX) NULL,
    [ExceptionStackTrace] NVARCHAR (MAX) NULL,
    [ExceptionHash]       AS             (checksum([ExceptionMessage],[ExceptionStackTrace])),
    CONSTRAINT [PK_Support.RequestLogExceptions] PRIMARY KEY CLUSTERED ([ExcptionId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIDX_RequestLogExceptions_ExceptionHash]
    ON [Support].[RequestLogExceptions]([ExceptionHash] ASC);

