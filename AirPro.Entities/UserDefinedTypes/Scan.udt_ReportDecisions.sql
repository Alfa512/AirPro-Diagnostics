
CREATE TYPE Scan.udt_ReportDecisions AS TABLE
(
	DecisionId INT NULL,
	DecisionText NVARCHAR(MAX) NOT NULL,
	DecisionTextSeverity INT NULL
)
GO