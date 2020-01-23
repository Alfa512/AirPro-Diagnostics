
CREATE PROCEDURE Billing.usp_SavePricingPlanLineItem
	@PricingPlanId INT
	,@PlanGroup VARCHAR(16)
	,@TypeId INT
	,@DomesticCost DECIMAL(18, 2)
	,@EuropeanCost DECIMAL(18, 2)
	,@AsianCost DECIMAL(18, 2)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	-- Build Updates Table.
	IF (OBJECT_ID('tempdb..#Updates') IS NOT NULL) DROP TABLE #Updates
	SELECT
		PricingPlanId
		,PlanGroup
		,TypeId
		,vmt.VehicleMakeTypeId
		,ISNULL(LineItemCost, 0.00) [LineItemCost]
	INTO #Updates
	FROM
	(
		SELECT
			@PricingPlanId [PricingPlanId]
			,@PlanGroup [PlanGroup]
			,@TypeId [TypeId]
			,@DomesticCost [DomesticCost]
			,@EuropeanCost [EuropeanCost]
			,@AsianCost [AsianCost]
	) p
	UNPIVOT
	(LineItemCost FOR VehicleMakeName
		IN (DomesticCost, EuropeanCost, AsianCost)) upvt
	INNER JOIN Repair.VehicleMakeTypes vmt
		ON REPLACE(upvt.VehicleMakeName, 'Cost', '') = vmt.VehicleMakeTypeName

	BEGIN TRAN

		BEGIN TRY

			-- Insert/Update Request Types.
			INSERT INTO Billing.PricingPlanRequestTypes
			(
				PricingPlanId
				,RequestTypeId
				,VehicleMakeTypeId
				,LineItemCost
			)
			SELECT
				u.PricingPlanId
				,u.TypeId
				,u.VehicleMakeTypeId
				,u.LineItemCost
			FROM #Updates u
			LEFT JOIN Billing.PricingPlanRequestTypes rt
				ON u.PricingPlanId = rt.PricingPlanId
					AND u.TypeId = rt.RequestTypeId
					AND u.VehicleMakeTypeId = rt.VehicleMakeTypeId
			WHERE u.PlanGroup = 'RequestType'
				AND rt.PricingPlanRequestTypeId IS NULL

			UPDATE rt
				SET rt.LineItemCost = u.LineItemCost
			FROM #Updates u
			INNER JOIN Billing.PricingPlanRequestTypes rt
				ON u.PricingPlanId = rt.PricingPlanId
					AND u.TypeId = rt.RequestTypeId
					AND u.VehicleMakeTypeId = rt.VehicleMakeTypeId
			WHERE u.PlanGroup = 'RequestType'
				AND u.LineItemCost != rt.LineItemCost

			-- Insert/Update Work Types.
			INSERT INTO Billing.PricingPlanWorkTypes
			(
				PricingPlanId
				,WorkTypeId
				,VehicleMakeTypeId
				,LineItemCost
			)
			SELECT
				u.PricingPlanId
				,u.TypeId
				,u.VehicleMakeTypeId
				,u.LineItemCost
			FROM #Updates u
			LEFT JOIN Billing.PricingPlanWorkTypes wt
				ON u.PricingPlanId = wt.PricingPlanId
					AND u.TypeId = wt.WorkTypeId
					AND u.VehicleMakeTypeId = wt.VehicleMakeTypeId
			WHERE u.PlanGroup = 'WorkType'
				AND wt.PricingPlanWorkTypeId IS NULL

			UPDATE wt
				SET wt.LineItemCost = u.LineItemCost
			FROM #Updates u
			INNER JOIN Billing.PricingPlanWorkTypes wt
				ON u.PricingPlanId = wt.PricingPlanId
					AND u.TypeId = wt.WorkTypeId
					AND u.VehicleMakeTypeId = wt.VehicleMakeTypeId
			WHERE u.PlanGroup = 'WorkType'
				AND u.LineItemCost != wt.LineItemCost

		END TRY

		BEGIN CATCH

			IF (@@TRANCOUNT > 0) ROLLBACK TRAN;

			THROW;

		END CATCH

	IF (@@TRANCOUNT > 0) COMMIT TRAN;

END
GO