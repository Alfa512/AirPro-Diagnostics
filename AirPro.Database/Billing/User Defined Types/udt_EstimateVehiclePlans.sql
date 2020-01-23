CREATE TYPE [Billing].[udt_EstimateVehiclePlans] AS TABLE (
    [VehicleMakeId]  INT             NOT NULL,
    [CompletionCost] DECIMAL (18, 2) NOT NULL);