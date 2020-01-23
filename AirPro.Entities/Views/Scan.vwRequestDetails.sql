DROP VIEW IF EXISTS Scan.vwRequestDetails;
GO

CREATE VIEW Scan.vwRequestDetails
WITH SCHEMABINDING
AS
	SELECT
		r.OrderId [RepairId]
		,r.RequestId
		,r.ReportId
		,r.RequestTypeId
		,r.RequestCategoryId
		,r.CreatedDt [RequestCreatedDt]
		,rt.TypeName [RequestTypeName]
		,CAST(r.OrderId AS NVARCHAR(MAX))
			+ '|' + ISNULL(CAST(r.RequestId AS NVARCHAR(10)), '')
			+ '|' + ISNULL(r.OtherWarningInfo, '')
			+ '|' + ISNULL(r.ProblemDescription, '')
			+ '|' + ISNULL(r.Notes, '')
			+ '|' + ISNULL(rt.TypeName, '') [SearchText]
	FROM Scan.Requests r
	INNER JOIN Scan.RequestTypes rt
		ON r.RequestTypeId = rt.RequestTypeId
GO

CREATE UNIQUE CLUSTERED INDEX PK_ScanRequestDetails ON Scan.vwRequestDetails (RequestId);
GO

CREATE NONCLUSTERED INDEX IX_ScanRequestDetails_RepairId ON Scan.vwRequestDetails (RepairId);
GO

CREATE FULLTEXT INDEX ON Scan.vwRequestDetails (SearchText) KEY INDEX PK_ScanRequestDetails;
GO
