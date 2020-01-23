CREATE TABLE [Support].[ConnectionLogs] (
    [ConnectionGuid]    UNIQUEIDENTIFIER   NOT NULL,
    [UserGuid]          UNIQUEIDENTIFIER   NOT NULL,
    [PageUrl]           NVARCHAR (MAX)     NULL,
    [ConnectionStartDt] DATETIMEOFFSET (7) NOT NULL,
    [ConnectionEndDt]   DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Support.ConnectionLogs] PRIMARY KEY CLUSTERED ([ConnectionGuid] ASC)
);




GO



GO


