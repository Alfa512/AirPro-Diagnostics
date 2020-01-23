CREATE PROCEDURE [Billing].[usp_SaveEstimatePlan]
	@EstimatePlanId INT,
	@Name NVARCHAR(128),
	@Description NVARCHAR(MAX),
	@ActiveInd BIT,
	@CurrentUser NVARCHAR(64),
	@EstimateVehiclePlans [Billing].[udt_EstimateVehiclePlans] readonly

	/*
		Author: Manuel Sauceda
		Date: 2018-03-18
		Description: Will commit a Merge operation based on the parameters
		Usage:
			DECLARE
				@EstimatePlanId INT = 6,
				@Name NVARCHAR(128) = 'Default 1',
				@Description NVARCHAR(MAX) = 'Some Desc',
				@ActiveInd BIT = 1,
				@CurrentUser NVARCHAR(64) = 'DB97F422-2814-E811-9E59-000D3A75F92D',
				@EstimateVehiclePlans [Billing].[udt_EstimateVehiclePlans]
	
				INSERT INTO @EstimateVehiclePlans VALUES (1, '55.00')
				INSERT INTO @EstimateVehiclePlans VALUES (2, '20.00')
				INSERT INTO @EstimateVehiclePlans VALUES (3, '15.00')
				INSERT INTO @EstimateVehiclePlans VALUES (4, '0.00')

				EXEC [Billing].[usp_SaveEstimatePlan] @EstimatePlanId, @Name, @Description, @ActiveInd, @CurrentUser, @EstimateVehiclePlans
	*/
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRAN cTran;
	
	DECLARE @CurrentDt DATETIMEOFFSET(7) = GETUTCDATE()
	DECLARE @PlanId INT
	DECLARE @output table([action] nvarchar(20), [EstimatePlanId] INT)
		
	BEGIN TRY
		--Estimate Plans
		MERGE [Billing].[EstimatePlans] AS T
		USING (SELECT @EstimatePlanId AS [EstimatePlanId], @Name AS [Name]
					, @Description AS [Description], @ActiveInd AS [ActiveInd]) AS S
		ON (T.[EstimatePlanId] = S.[EstimatePlanId])
		WHEN MATCHED THEN
			UPDATE SET T.[Name] = S.[Name], T.[Description] = S.[Description], 
					   T.[ActiveInd] = S.[ActiveInd],
					   T.UpdatedByUserGuid = @CurrentUser, T.UpdatedDt = GETUTCDATE()
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Name], [Description], ActiveInd, CreatedByUserGUid, CreatedDt, UpdatedByUserGuid, UpdatedDt)
			VALUES (S.[Name], S.[Description], S.ActiveInd, @CurrentUser, @CurrentDt, @CurrentUser, @CurrentDt)
		OUTPUT $action, inserted.EstimatePlanId INTO @output;

		SELECT TOP 1 @PlanId = ISNULL([EstimatePlanId], @EstimatePlanId) FROM @output
		
		--Merge Vehicles
		MERGE [Billing].[EstimatePlanVehicles] AS T
		USING (SELECT @PlanId AS [EstimatePlanId], VehicleMakeId, CompletionCost FROM @EstimateVehiclePlans) AS S
		ON (T.EstimatePlanId = S.EstimatePlanId AND T.VehicleMakeId = S.[VehicleMakeId])
		WHEN MATCHED THEN
			UPDATE SET T.CompletionCost = S.CompletionCost
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (EstimatePlanId, VehicleMakeId, CompletionCost)
			VALUES (S.EstimatePlanId, S.VehicleMakeId, S.CompletionCost)
		WHEN NOT MATCHED BY SOURCE AND T.EstimatePlanId = @PlanId THEN DELETE
		OUTPUT $action, @PlanId INTO @output;

		COMMIT TRAN cTran;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN cTran;
		THROW
	END CATCH

	SELECT @PlanId
END