CREATE TABLE [Service].[MitchellReports] (
    [MitchellReportId] INT                IDENTITY (1, 1) NOT NULL,
    [RequestId]        INT                NOT NULL,
    [RequestUserGuid]  UNIQUEIDENTIFIER   NOT NULL,
    [RequestDt]        DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_Service.MitchellReports] PRIMARY KEY CLUSTERED ([MitchellReportId] ASC),
    CONSTRAINT [FK_Service.MitchellReports_Access.Users_RequestUserGuid] FOREIGN KEY ([RequestUserGuid]) REFERENCES [Access].[Users] ([UserGuid]),
    CONSTRAINT [FK_Service.MitchellReports_Scan.Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Scan].[Requests] ([RequestId])
);


GO
CREATE NONCLUSTERED INDEX [IX_RequestUserGuid]
    ON [Service].[MitchellReports]([RequestUserGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RequestId]
    ON [Service].[MitchellReports]([RequestId] ASC);

