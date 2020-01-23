/****** Object:  UserDefinedTableType [dbo].[udt_EstimatePlans]    Script Date: 3/18/2018 9:20:21 PM ******/
CREATE TYPE [Billing].[udt_EstimateVehiclePlans] AS TABLE(
	[VehicleMakeId] [int] NOT NULL,
	[CompletionCost] [DECIMAL](18, 2) NOT NULL
)
GO