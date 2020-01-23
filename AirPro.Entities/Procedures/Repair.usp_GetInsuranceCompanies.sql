CREATE PROCEDURE [Repair].[usp_GetInsuranceCompanies]
	@search VARCHAR(MAX) = NULL
	, @InsuranceCompanyId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SET @Search = '%' + ISNULL(@Search, '') + '%';
	SET @InsuranceCompanyId = NULLIF(@InsuranceCompanyId, 0);

	SELECT * FROM Repair.InsuranceCompanies I WITH(NOLOCK)
	WHERE (@InsuranceCompanyId IS NOT NULL AND I.InsuranceCompanyId = @InsuranceCompanyId)
		OR
		(
			@InsuranceCompanyId IS NULL
			AND
			(
				I.InsuranceCompanyName LIKE @Search
				OR I.ProgramName LIKE @search
			)
		)

	SELECT CCCInsuranceCompanyId FROM Service.CCCInsuranceCompanies
	WHERE RepairInsuranceCompanyId = @InsuranceCompanyId
END
GO