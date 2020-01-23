
CREATE PROCEDURE Scan.usp_SaveRequestType
	@RequestTypeId INT
	,@RequestTypeName NVARCHAR(100)
	,@ActiveFlag BIT = 1
	,@BillableFlag BIT = 0
	,@SortOrder INT = NULL
	,@DefaultPrice FLOAT = NULL
	,@InvoiceMemo NVARCHAR(800)
	,@Instructions NVARCHAR(800)
	,@RequestCategoryIds NVARCHAR(MAX) = NULL
	,@ValidationRuleIds NVARCHAR(MAX) = NULL
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	IF NULLIF(RTRIM(@RequestTypeName), '') IS NULL
		THROW 50000, 'Request Type Name is Empty.', 1;

	BEGIN TRAN
	
		BEGIN TRY

			/**************************************************
				Update Request Type.
			**************************************************/
			UPDATE rt
				SET rt.TypeName = @RequestTypeName
					,rt.ActiveFlag = ISNULL(@ActiveFlag, 1)
					,rt.BillableFlag = ISNULL(@BillableFlag, 0)
					,rt.SortOrder = ISNULL(@SortOrder, rt.SortOrder)
					,rt.DefaultPrice = ISNULL(@DefaultPrice, 0)
					,rt.InvoiceMemo = @InvoiceMemo
					,rt.Instructions = @Instructions
			FROM Scan.RequestTypes rt
			WHERE rt.RequestTypeId = @RequestTypeId

			/**************************************************
				Update Request Type Categories.
			**************************************************/
			DECLARE @RequestCategoryTypeUpdates TABLE
				(RequestTypeId INT NULL, RequestCategoryId INT NULL);
			MERGE Scan.RequestCategoryTypes AS t
			USING
			(
				SELECT
					@RequestTypeId [RequestTypeId]
					,c.Val [RequestCategoryId]
				FROM Common.udf_CommaListToTable(@RequestCategoryIds) c
				WHERE NULLIF(RTRIM(c.Val), '') IS NOT NULL
			) AS s
			ON (t.RequestTypeId = s.RequestTypeId AND t.RequestCategoryId = s.RequestCategoryId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (RequestTypeId, RequestCategoryId)
				VALUES (s.RequestTypeId, s.RequestCategoryId)
			WHEN NOT MATCHED BY SOURCE AND t.RequestTypeId = @RequestTypeId THEN
				DELETE
			OUTPUT INSERTED.RequestTypeId, INSERTED.RequestCategoryId
			INTO @RequestCategoryTypeUpdates;

			/**************************************************
				Update Request Type Validation Rules.
			**************************************************/
			DECLARE @RequestTypeValidationRuleUpdates TABLE
				(RequestTypeId INT NULL, ValidationRuleId INT NULL);
			MERGE Scan.RequestTypeValidationRules AS t
			USING
			(
				SELECT
					@RequestTypeId [RequestTypeId]
					,r.Val [ValidationRuleId]
				FROM Common.udf_CommaListToTable(@ValidationRuleIds) r
				WHERE NULLIF(RTRIM(r.Val), '') IS NOT NULL
			) AS s
			ON (t.RequestTypeId = s.RequestTypeId AND t.ValidationRuleId = s.ValidationRuleId)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (RequestTypeId, ValidationRuleId)
				VALUES (s.RequestTypeId, s.ValidationRuleId)
			WHEN NOT MATCHED BY SOURCE AND t.RequestTypeId = @RequestTypeId THEN
				DELETE
			OUTPUT INSERTED.RequestTypeId, INSERTED.ValidationRuleId
			INTO @RequestTypeValidationRuleUpdates;

		END TRY
		BEGIN CATCH

			IF @@TRANCOUNT > 0 ROLLBACK TRAN;

			THROW;

		END CATCH

	IF @@TRANCOUNT > 0 COMMIT TRAN;

	/**************************************************
		Return Results
	**************************************************/
	SELECT @RequestTypeId [RequestTypeId];
	RETURN @RequestTypeId;

END