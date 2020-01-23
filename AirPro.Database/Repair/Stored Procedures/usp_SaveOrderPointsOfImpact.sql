CREATE PROCEDURE [Repair].[usp_SaveOrderPointsOfImpact]
	@OrderId INT
	,@PointsOfImpact NVARCHAR(MAX)

	/*
		Author: Manuel Sauceda
		Date: 2018-03-06
		Description: Will commit a Merge operation based on the parameters
		Usage:
			DECLARE 
				@OrderId INT = 10
				,@PointsOfImpact NVARCHAR(MAX) = '1,2'
			
			EXEC [Repair].[usp_SaveOrderPointsOfImpact] @OrderId, @PointsOfImpact
	*/
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRAN cTran;

	BEGIN TRY
		DECLARE @output table([action] nvarchar(20))
		
		DECLARE @PointsOfImpactTable TABLE (Id INT)
        INSERT INTO @PointsOfImpactTable
        SELECT Id FROM Common.udf_IdListToTable(@PointsOfImpact)


		--Merge PointsOfImpact
		MERGE [Repair].[OrderPointOfImpacts] AS T
		USING (SELECT @OrderId AS [OrderId], [Id] as [Val] FROM @PointsOfImpactTable) AS S
		ON (T.OrderId = S.[OrderId] AND T.PointOfImpactId = S.Val)
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (OrderId, PointOfImpactId)
			VALUES (S.OrderId, S.Val)
		WHEN NOT MATCHED BY SOURCE AND T.OrderID = @OrderId THEN DELETE
		OUTPUT $action INTO @output;
	
		COMMIT TRAN cTran;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN cTran;
		THROW
	END CATCH
END