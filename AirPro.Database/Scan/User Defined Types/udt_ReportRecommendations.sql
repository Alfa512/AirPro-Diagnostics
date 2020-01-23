CREATE TYPE [Scan].[udt_ReportRecommendations] AS TABLE (
    [ReportOrderTroubleCodeId]      BIGINT          NULL,
    [ControllerId]                  INT             NULL,
    [ControllerIdOrig]              INT             NULL,
    [ControllerName]                NVARCHAR (200)  NULL,
    [ControllerNameOrig]            NVARCHAR (200)  NULL,
    [TroubleCodeId]                 INT             NULL,
    [TroubleCodeIdOrig]             INT             NULL,
    [TroubleCode]                   NVARCHAR (20)   NULL,
    [TroubleCodeOrig]               NVARCHAR (20)   NULL,
    [TroubleCodeDescription]        NVARCHAR (1000) NULL,
    [TroubleCodeDescriptionOrig]    NVARCHAR (1000) NULL,
    [ResultTroubleCodeId]           BIGINT          NULL,
    [InformCustomerInd]             BIT             NULL,
    [AccidentRelatedInd]            BIT             NULL,
    [ExcludeFromReportInd]          BIT             NULL,
    [CodeClearedInd]                BIT             NULL,
    [TroubleCodeNoteText]           NVARCHAR (MAX)  NULL,
    [TroubleCodeRecommendationId]   INT             NULL,
    [TroubleCodeRecommendationText] NVARCHAR (MAX)  NULL,
    [RecommendationTextSeverity]    INT             NULL);





