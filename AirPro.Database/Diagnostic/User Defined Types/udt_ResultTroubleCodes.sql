CREATE TYPE [Diagnostic].[udt_ResultTroubleCodes] AS TABLE (
    [ControllerId]           INT             NULL,
    [ControllerName]         NVARCHAR (400)  NOT NULL,
    [TroubleCodeId]          INT             NULL,
    [TroubleCode]            NVARCHAR (40)   NULL,
    [TroubleCodeDescription] NVARCHAR (2000) NOT NULL,
    [TroubleCodeInformation] NVARCHAR (MAX)  NULL);

