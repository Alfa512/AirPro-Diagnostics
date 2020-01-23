CREATE TYPE [Technician].[udt_TimeOff] AS TABLE (
    [TimeOffEntryId] INT            NOT NULL,
    [StartDate]      DATETIME       NOT NULL,
    [EndDate]        DATETIME       NOT NULL,
    [Reason]         NVARCHAR (500) NULL,
    [DeleteInd]      BIT            NOT NULL);



