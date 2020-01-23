CREATE TYPE Scan.udt_ReportRecommendations AS TABLE
(
	ReportOrderTroubleCodeId BIGINT
	,ControllerId INT
	,ControllerIdOrig INT
	,ControllerName NVARCHAR(200)
	,ControllerNameOrig NVARCHAR(200)
	,TroubleCodeId INT
	,TroubleCodeIdOrig INT
	,TroubleCode NVARCHAR(20)
	,TroubleCodeOrig NVARCHAR(20)
	,TroubleCodeDescription NVARCHAR(1000)
	,TroubleCodeDescriptionOrig NVARCHAR(1000)
	,ResultTroubleCodeId BIGINT
	,InformCustomerInd BIT
	,AccidentRelatedInd BIT
	,ExcludeFromReportInd BIT
	,CodeClearedInd BIT
	,TroubleCodeNoteText NVARCHAR(MAX)
	,TroubleCodeRecommendationId INT
	,TroubleCodeRecommendationText NVARCHAR(MAX)
	,RecommendationTextSeverity INT
)
GO