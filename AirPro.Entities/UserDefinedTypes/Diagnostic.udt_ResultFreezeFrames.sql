
CREATE TYPE Diagnostic.udt_ResultFreezeFrames AS TABLE
(
	ControllerId INT NULL,
	ControllerName NVARCHAR(400) NOT NULL,
	FreezeFrameId INT NULL,
	FreezeFrameTroubleCode NVARCHAR(200) NOT NULL,
	SensorGroupsJson NVARCHAR(MAX) NULL
)
GO
