CREATE PROCEDURE [Repair].[usp_SaveInsuranceCompany]
	@InsuranceCompanyId int = 0,
	@InsuranceCompanyName VARCHAR(MAX),
	@InsuranceCompanyCccIds NVARCHAR(MAX),
	@ProgramName NVARCHAR(128),
	@DisabledInd bit
	
	/**
	***	Author: Manuel Sauceda
	***	Date: 2018-02-20
	***	Description: Will commit a Merge operation based on the parameters, 
	***		Specify InsuranceCompanyId = 0 to Insert
	***		Specify InsuranceCompanyId = [Any Other Value] to Update
	***	Usage:
	***		DECLARE @InsuranceCompanyId int = 0,
	***			@InsuranceCompanyName VARCHAR(MAX) = 'Manuel',
	***			@InsuranceCompanyCcc NVARCHAR(128) = 'Something',
	***			@ProgramName NVARCHAR(128) = 'Program 1'
	***
	***		EXEC [Repair].[Usp_SaveInsuranceCompany] @InsuranceCompanyId, @InsuranceCompanyName, @InsuranceCompanyCcc, @ProgramName
	**/

	/**
	2018-05-01. Added Program Name
	**/
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRAN cTran;

	BEGIN TRY
		MERGE [Repair].[InsuranceCompanies] AS T
		USING (SELECT	@InsuranceCompanyId AS [CompanyId], 
						@InsuranceCompanyName AS [CompanyName], 
						@DisabledInd AS [DisabledInd],
						@ProgramName AS [ProgramName]) AS S
		ON (T.InsuranceCompanyId = S.CompanyId)
		WHEN MATCHED THEN
			UPDATE SET T.InsuranceCompanyName = S.CompanyName, T.ProgramName = S.ProgramName, T.DisabledInd = S.DisabledInd
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (InsuranceCompanyName, ProgramName, DisabledInd)
			VALUES (S.CompanyName, S.ProgramName, S.DisabledInd)
		OUTPUT $action, Inserted.*;
		
		UPDATE Service.CCCInsuranceCompanies
		SET  RepairInsuranceCompanyId = CASE WHEN (CCCInsuranceCompanyId IN (SELECT value FROM STRING_SPLIT(@InsuranceCompanyCccIds, ','))) 
		                                     THEN @InsuranceCompanyId ELSE NULL END

		COMMIT TRAN cTran;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN cTran;
		THROW
	END CATCH
END