CREATE TABLE [Support].[ApplicationExceptions] (
    [ExceptionId]         INT                IDENTITY (1, 1) NOT NULL,
    [ExceptionMessage]    NVARCHAR (MAX)     NULL,
    [ExceptionStackTrace] NVARCHAR (MAX)     NULL,
    [ExceptionOccuredDt]  DATETIMEOFFSET (7) NOT NULL,
    [ExceptionParentId]   INT                NULL,
    [ExceptionObjectInfo] NVARCHAR (MAX)     NULL,
    CONSTRAINT [PK_Support.ApplicationExceptions] PRIMARY KEY CLUSTERED ([ExceptionId] ASC)
);








GO
CREATE NONCLUSTERED INDEX [IX_ExceptionParentId]
    ON [Support].[ApplicationExceptions]([ExceptionParentId] ASC);

