
CREATE PROCEDURE Billing.usp_GetPricingPlanTemplate
AS
BEGIN

	SET NOCOUNT ON;

	WITH
	BasePlan
	AS
	(
		SELECT
			i.PlanGroup
			,i.TypeId
			,i.TypeGroup
			,i.TypeName
			,vmt.VehicleMakeTypeId
			,0.00 [LineItemCost]
			,i.SortOrder
		FROM
		(
			SELECT
				'RequestType' [PlanGroup]
				,rt.RequestTypeId [TypeId]
				,rt.TypeName [TypeGroup]
				,rt.TypeName
				,rt.SortOrder
			FROM Scan.RequestTypes rt

			UNION

			SELECT
				'WorkType' [PlanGroup]
				,wt.WorkTypeId [TypeId]
				,wtg.WorkTypeGroupName [TypeGroup]
				,wt.WorkTypeName [TypeName]
				,ROW_NUMBER() OVER (ORDER BY wtg.WorkTypeGroupSortOrder, wt.WorkTypeSortOrder)
			FROM Scan.WorkTypes wt
			INNER JOIN Scan.WorkTypeGroups wtg
				ON wt.WorkTypeGroupId = wtg.WorkTypeGroupId
		) i
		CROSS APPLY
		(
			SELECT VehicleMakeTypeId
			FROM Repair.VehicleMakeTypes
		) vmt
	)
	,LineItems
	AS
	(
		SELECT
			PlanGroup
			,TypeId
			,TypeGroup
			,TypeName
			,Domestic [DomesticCost]
			,European [EuropeanCost]
			,Asian [AsianCost]
			,SortOrder
		FROM
		(
			SELECT
				PlanGroup
				,TypeId
				,TypeGroup
				,TypeName
				,vmt.VehicleMakeTypeName
				,i.LineItemCost
				,i.SortOrder
			FROM BasePlan i
			INNER JOIN Repair.VehicleMakeTypes vmt
				ON i.VehicleMakeTypeId = vmt.VehicleMakeTypeId
		) p
		PIVOT
		(
			SUM(LineItemCost)
			FOR VehicleMakeTypeName IN ([Domestic], [European], [Asian])
		) AS pt
	)

	SELECT
		li.PlanGroup
		,li.TypeId
		,li.TypeGroup
		,li.TypeName
		,li.DomesticCost
		,li.EuropeanCost
		,li.AsianCost
	FROM LineItems li
	ORDER BY
		li.PlanGroup
		,li.SortOrder

END