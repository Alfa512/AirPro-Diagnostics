CREATE TYPE [Technician].[udt_Schedules] AS TABLE (
    [DayOfWeek]   INT      NOT NULL,
    [StartTime]   TIME (7) NULL,
    [EndTime]     TIME (7) NULL,
    [BreakStart] TIME (7) NULL,
    [BreakEnd]   TIME (7) NULL);





