CREATE TABLE [Reporting].[ReportDataLoads] (
    [ReportDataLoadId] INT                IDENTITY (1, 1) NOT NULL,
    [ReportDataCount]  INT                DEFAULT ((0)) NOT NULL,
    [ReportViewCount]  INT                DEFAULT ((0)) NOT NULL,
    [InsertedCount]    INT                DEFAULT ((0)) NOT NULL,
    [DeletedCount]     INT                DEFAULT ((0)) NOT NULL,
    [LogStartDt]       DATETIMEOFFSET (7) DEFAULT (getutcdate()) NOT NULL,
    [LogEndDt]         DATETIMEOFFSET (7) NULL,
    [SuccessInd]       BIT                DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ReportDataLoadId] ASC)
);

