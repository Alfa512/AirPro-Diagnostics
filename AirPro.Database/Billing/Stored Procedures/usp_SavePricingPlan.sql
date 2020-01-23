CREATE PROCEDURE [Billing].[usp_SavePricingPlan]
	@PricingPlanId INT
	,@PricingPlanName NVARCHAR(MAX)
	,@PricingPlanDescription NVARCHAR(MAX)
	,@CurrencyId INT
	,@PricingPlanActiveInd BIT
	,@UserGuid UNIQUEIDENTIFIER

	/*
		DECLARE @PricingPlanId INT = 0
			,@PricingPlanName NVARCHAR(MAX) = 'Default Plan (Copy) (CAD)'
			,@PricingPlanDescription NVARCHAR(MAX) = 'Default Pricing Plan'
			,@CurrencyId INT = 2
			,@PricingPlanActiveInd BIT = 0
			,@UserGuid UNIQUEIDENTIFIER = 'db97f422-2814-e811-9e59-000d3a75f92d'
	
		EXEC [Billing].[usp_SavePricingPlan] @PricingPlanId, @PricingPlanName, @PricingPlanDescription, @CurrencyId, @PricingPlanActiveInd, @UserGuid
	*/
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			IF (NULLIF(@PricingPlanId, 0) IS NULL)
				OR NOT EXISTS (SELECT 1 FROM Billing.PricingPlans WHERE PricingPlanId = @PricingPlanId)
				BEGIN

					INSERT INTO Billing.PricingPlans
					(
						PricingPlanName
						,PricingPlanDescription
						,PricingPlanActiveInd
						,CreatedByUserGuid
						,CreatedDt
						,CurrencyId
					)
					VALUES
					(
						@PricingPlanName
						,@PricingPlanDescription
						,@PricingPlanActiveInd
						,@UserGuid
						,GETUTCDATE()
						,@CurrencyId
					)

					SET @PricingPlanId = SCOPE_IDENTITY()

				END
			ELSE
				BEGIN

					UPDATE p
						SET p.PricingPlanName = @PricingPlanName
							,p.PricingPlanDescription = @PricingPlanDescription
							,p.CurrencyId = @CurrencyId
							,p.PricingPlanActiveInd = @PricingPlanActiveInd
							,p.UpdatedByUserGuid = @UserGuid
							,p.UpdatedDt = GETUTCDATE()
					FROM Billing.PricingPlans p
					WHERE p.PricingPlanId = @PricingPlanId

				END

		END TRY
		
		BEGIN CATCH

			IF (@@TRANCOUNT > 0) ROLLBACK TRAN;

			THROW;

		END CATCH

	IF (@@TRANCOUNT > 0) COMMIT TRAN;

	SELECT @PricingPlanId;

	RETURN @PricingPlanId;

END