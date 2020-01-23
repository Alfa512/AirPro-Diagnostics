CREATE TABLE [Support].[RequestLogs] (
    [RequestLogId]      BIGINT             IDENTITY (1, 1) NOT NULL,
    [UserGuid]          UNIQUEIDENTIFIER   NULL,
    [SessionId]         NVARCHAR (24)      NULL,
    [UserAgentId]       INT                NOT NULL,
    [UserAddress]       NVARCHAR (45)      NULL,
    [RawUrl]            NVARCHAR (MAX)     NULL,
    [RouteUrl]          NVARCHAR (100)     NULL,
    [RouteArea]         NVARCHAR (100)     NULL,
    [RouteController]   NVARCHAR (100)     NULL,
    [RouteAction]       NVARCHAR (100)     NULL,
    [RouteId]           NVARCHAR (100)     NULL,
    [RequestMethod]     NVARCHAR (10)      NULL,
    [ActionStartTime]   DATETIMEOFFSET (7) NULL,
    [ActionEndTime]     DATETIMEOFFSET (7) NULL,
    [ResultStartTime]   DATETIMEOFFSET (7) NULL,
    [ResultEndTime]     DATETIMEOFFSET (7) NULL,
    [ActionExceptionId] INT                NULL,
    [ResultExceptionId] INT                NULL,
    CONSTRAINT [PK_Support.RequestLogs] PRIMARY KEY CLUSTERED ([RequestLogId] ASC),
    CONSTRAINT [FK_Support.RequestLogs_Access.Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Support.RequestLogs_Support.RequestLogExceptions_ActionExceptionId] FOREIGN KEY ([ActionExceptionId]) REFERENCES [Support].[RequestLogExceptions] ([ExcptionId]),
    CONSTRAINT [FK_Support.RequestLogs_Support.RequestLogExceptions_ResultExceptionId] FOREIGN KEY ([ResultExceptionId]) REFERENCES [Support].[RequestLogExceptions] ([ExcptionId]),
    CONSTRAINT [FK_Support.RequestLogs_Support.RequestLogUserAgents_UserAgentId] FOREIGN KEY ([UserAgentId]) REFERENCES [Support].[RequestLogUserAgents] ([UserAgentId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ResultExceptionId]
    ON [Support].[RequestLogs]([ResultExceptionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ActionExceptionId]
    ON [Support].[RequestLogs]([ActionExceptionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserAgentId]
    ON [Support].[RequestLogs]([UserAgentId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SessionId]
    ON [Support].[RequestLogs]([SessionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserGuid]
    ON [Support].[RequestLogs]([UserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SupportRequestLogs_Route]
    ON [Support].[RequestLogs]([RouteArea] ASC, [RouteController] ASC, [RouteAction] ASC, [RouteId] ASC)
    INCLUDE([UserGuid], [ActionStartTime]);

