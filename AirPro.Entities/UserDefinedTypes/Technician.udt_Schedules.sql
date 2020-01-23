CREATE TYPE [Technician].[udt_Schedules] AS TABLE(
	[DayOfWeek] [INT] NOT NULL,
	[StartTime] [TIME] NULL,
	[EndTime] [TIME] NULL,
	[BreakStart] [TIME] NULL,
	[BreakEnd] [TIME] NULL
)
GO