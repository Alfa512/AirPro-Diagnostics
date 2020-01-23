CREATE TRIGGER [Access].[trgAccessAccountsArchive] ON [Access].[Accounts]
AFTER UPDATE, DELETE
AS
BEGIN

	INSERT INTO Access.AccountsArchive
	(
		AccountGuid
		,Name
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,Address1
		,Address2
		,City
		,StateId
		,Zip
		,Phone
		,Fax
		,DiscountPercentage
		,ActiveInd
		,EmployeeGuid
	)
	SELECT
		AccountGuid
		,Name
		,CreatedByUserGuid
		,CreatedDt
		,UpdatedByUserGuid
		,UpdatedDt
		,Address1
		,Address2
		,City
		,StateId
		,Zip
		,Phone
		,Fax
		,DiscountPercentage
		,ActiveInd
		,EmployeeGuid
	FROM deleted

END
GO

ALTER TABLE [Access].[Accounts] ENABLE TRIGGER [trgAccessAccountsArchive]
GO