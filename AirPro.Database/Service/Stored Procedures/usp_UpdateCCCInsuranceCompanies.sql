
CREATE PROCEDURE Service.usp_UpdateCCCInsuranceCompanies
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN

		BEGIN TRY

			DECLARE @InsuranceCompanyUpdates TABLE
			(
				CCCInsuranceCompanyId VARCHAR(128)
				,CCCInsuranceCompanyName VARCHAR(MAX)
			);

			MERGE Service.CCCInsuranceCompanies AS t
			USING
			(
				SELECT
					i.InsuranceCompanyId
					,n.InsuranceCompanyName
				FROM
				(
					SELECT InsuranceCompanyId
					FROM Service.CCCEstimates
					WHERE InsuranceCompanyId IS NOT NULL
					GROUP BY InsuranceCompanyId
				) i
				OUTER APPLY
				(
					SELECT TOP 1 InsuranceCompanyName
					FROM Service.CCCEstimates
					WHERE InsuranceCompanyId = i.InsuranceCompanyId
					ORDER BY EstimateId DESC
				) n
			) AS s
			ON (t.CCCInsuranceCompanyId = s.InsuranceCompanyId)
			WHEN MATCHED THEN
				UPDATE SET t.CCCInsuranceCompanyName = s.InsuranceCompanyName
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (CCCInsuranceCompanyId, CCCInsuranceCompanyName)
				VALUES (InsuranceCompanyId, InsuranceCompanyName)
			WHEN NOT MATCHED BY SOURCE AND t.RepairInsuranceCompanyId IS NULL THEN
				DELETE
			OUTPUT INSERTED.CCCInsuranceCompanyId, INSERTED.CCCInsuranceCompanyName
			INTO @InsuranceCompanyUpdates;

		END TRY

		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

END