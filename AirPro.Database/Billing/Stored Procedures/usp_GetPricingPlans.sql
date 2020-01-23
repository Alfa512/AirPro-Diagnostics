CREATE PROCEDURE [Billing].[usp_GetPricingPlans]
	@Offset CHAR(10) = '-04:00'
	,@Search VARCHAR(MAX) = NULL
	,@PricingPlanId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SET @Search = '%' + ISNULL(@Search, '') + '%';
	SET @PricingPlanId = NULLIF(@PricingPlanId, 0);

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
	,PricingPlans
	AS
	(
		SELECT
			pp.PricingPlanId
			,b.PlanGroup
			,b.TypeId
			,b.TypeGroup
			,b.TypeName
			,b.VehicleMakeTypeId
			,ISNULL(i.LineItemCost, 0.00) [LineItemCost]
			,b.SortOrder
		FROM Billing.PricingPlans pp
		CROSS APPLY
		(
			SELECT
				PlanGroup
				,TypeId
				,TypeGroup
				,TypeName
				,VehicleMakeTypeId
				,SortOrder
			FROM BasePlan
		) b
		OUTER APPLY
		(
			SELECT pwt.LineItemCost
			FROM Billing.PricingPlanWorkTypes pwt
			WHERE pwt.PricingPlanId = pp.PricingPlanId
				AND b.PlanGroup = 'WorkType'
				AND b.TypeId = pwt.WorkTypeId
				AND b.VehicleMakeTypeId = pwt.VehicleMakeTypeId

			UNION

			SELECT prt.LineItemCost
			FROM Billing.PricingPlanRequestTypes prt
			WHERE prt.PricingPlanId = pp.PricingPlanId
				AND b.PlanGroup = 'RequestType'
				AND b.TypeId = prt.RequestTypeId
				AND b.VehicleMakeTypeId = prt.VehicleMakeTypeId
		) i
	)
	,LineItems
	AS
	(
		SELECT
			PricingPlanId
			,PlanGroup
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
				PricingPlanId
				,PlanGroup
				,TypeId
				,TypeGroup
				,TypeName
				,vmt.VehicleMakeTypeName
				,i.LineItemCost
				,i.SortOrder
			FROM PricingPlans i
			INNER JOIN Repair.VehicleMakeTypes vmt
				ON i.VehicleMakeTypeId = vmt.VehicleMakeTypeId
		) p
		PIVOT
		(
			SUM(LineItemCost)
			FOR VehicleMakeTypeName IN ([Domestic], [European], [Asian])
		) AS pt
	)
	,Final
	AS
	(
		SELECT
			p.PricingPlanId
			,p.PricingPlanName
			,p.PricingPlanDescription
			,p.PricingPlanActiveInd
			,p.CurrencyId
			,C.Name AS CurrencyName
			,Common.udf_GetDisplayName(cu.LastName, cu.FirstName) [CreatedByName]
			,Common.udf_GetLocalDateTime(p.CreatedDt, @Offset) [CreatedDateTime]
			,Common.udf_GetDisplayName(uu.LastName, uu.FirstName) [UpdatedByName]
			,Common.udf_GetLocalDateTime(p.UpdatedDt, @Offset) [UpdatedDateTime]
			,li.PlanGroup
			,li.TypeId
			,li.TypeGroup
			,li.TypeName
			,li.DomesticCost
			,li.EuropeanCost
			,li.AsianCost
			,li.SortOrder
		FROM Billing.PricingPlans p
		INNER JOIN LineItems li
			ON p.PricingPlanId = li.PricingPlanId
		INNER JOIN Access.Users cu
			ON p.CreatedByUserGuid = cu.UserGuid
		LEFT JOIN Access.Users uu
			ON p.UpdatedByUserGuid = uu.UserGuid
		LEFT JOIN Billing.Currencies C
			ON C.CurrencyId = p.CurrencyId
	)

	SELECT
		f.PricingPlanId
		,f.PricingPlanName
		,f.PricingPlanDescription
		,f.PricingPlanActiveInd
		,f.CurrencyId
		,f.CurrencyName
		,f.CreatedByName
		,f.CreatedDateTime
		,ISNULL(f.UpdatedByName, f.CreatedByName) [UpdatedByName]
		,ISNULL(f.UpdatedDateTime, f.CreatedDateTime) [UpdatedDateTime]
		,f.PlanGroup
		,f.TypeId
		,f.TypeGroup
		,f.TypeName
		,f.DomesticCost
		,f.EuropeanCost
		,f.AsianCost
	FROM Final f
	WHERE (@PricingPlanId IS NOT NULL AND f.PricingPlanId = @PricingPlanId)
		OR
		(
			@PricingPlanId IS NULL
			AND
			(
				f.PricingPlanName LIKE @Search
				OR f.PricingPlanDescription LIKE @Search
				OR f.CreatedByName LIKE @Search
				OR f.UpdatedByName LIKE @Search
				OR f.CurrencyName LIKE @Search
			)
		)
	ORDER BY
		f.PlanGroup
		,f.SortOrder

END