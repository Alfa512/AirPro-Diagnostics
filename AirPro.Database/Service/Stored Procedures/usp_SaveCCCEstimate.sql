
CREATE PROCEDURE Service.usp_SaveCCCEstimate
	@AppId INT
	,@Trigger VARCHAR(10)
	,@EstimateXml XML
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	WITH XMLNAMESPACES(DEFAULT 'http://www.cieca.com/BMS')
	INSERT INTO Service.CCCEstimates
	(
		[RequestGuid]
		,[AppId]
		,[Trigger]
		,[DocumentGuid]
		,[DocumentVersion]
		,[DocumentStatus]
		,[ShopId]
		,[ShopName]
		,[ShopRONumber]
		,[VehicleVIN]
		,[VehicleYear]
		,[VehicleMake]
		,[VehicleModel]
		,[VehicleOdometer]
		,[VehicleDrivable]
		,[InsuranceCompanyId]
		,[InsuranceCompanyName]
		,[RawXml]
		,[ProcessedInd]
		,[ProcessedDt]
	)
	SELECT
		x.[RequestGuid]
		,x.[AppId]
		,x.[Trigger]
		,x.[DocumentGuid]
		,x.[DocumentVersion]
		,x.[DocumentStatus]
		,x.[ShopId]
		,x.[ShopName]
		,x.[ShopRONumber]
		,x.[VehicleVIN]
		,x.[VehicleYear]
		,x.[VehicleMake]
		,x.[VehicleModel]
		,x.[VehicleOdometer]
		,x.[VehicleDrivable]
		,x.[InsuranceCompanyId]
		,x.[InsuranceCompanyName]
		,x.[RawXml]
		,CASE x.[Trigger] WHEN 'manual' THEN 0 ELSE 1 END [ProcessedInd]
		,CASE x.[Trigger] WHEN 'manual' THEN NULL ELSE GETUTCDATE() END [ProcessedDt]
	FROM
	(
		SELECT
			@EstimateXml.value('(/VehicleDamageEstimateAddRq/RqUID)[1]', 'UNIQUEIDENTIFIER') [RequestGuid]
			,@AppId [AppId]
			,@Trigger [Trigger]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/DocumentInfo/DocumentID)[1]', 'UNIQUEIDENTIFIER') [DocumentGuid]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/DocumentInfo/DocumentVer/DocumentVerNum)[1]', 'INT') [DocumentVersion]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/DocumentInfo/DocumentStatus)[1]', 'NCHAR(1)') [DocumentStatus]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/AdminInfo/RepairFacility/Party/OrgInfo/IDInfo/IDNum)[1]', 'NVARCHAR(128)') [ShopId]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/AdminInfo/RepairFacility/Party/OrgInfo/CompanyName)[1]', 'NVARCHAR(128)') [ShopName]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/DocumentInfo/ReferenceInfo/RepairOrderID)[1]', 'NVARCHAR(128)') [ShopRONumber]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/VehicleInfo/VINInfo/VIN/VINNum)[1]', 'NVARCHAR(18)') [VehicleVIN]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/VehicleInfo/VehicleDesc/ModelYear)[1]', 'NVARCHAR(4)') [VehicleYear]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/VehicleInfo/VehicleDesc/MakeDesc)[1]', 'NVARCHAR(128)') [VehicleMake]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/VehicleInfo/VehicleDesc/ModelName)[1]', 'NVARCHAR(128)') [VehicleModel]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/EventInfo/RepairEvent/ArrivalOdometerReading)[1]', 'NVARCHAR(9)') [VehicleOdometer]
			,CAST(CASE @EstimateXml.value('(/VehicleDamageEstimateAddRq/VehicleInfo/Condition/DrivableInd)[1]', 'NCHAR(1)') WHEN 'Y' THEN 1 ELSE 0 END AS BIT) [VehicleDrivable]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/AdminInfo/InsuranceCompany/Party/OrgInfo/IDInfo[IDQualifierCode="CompanyID"]/IDNum)[1]', 'NVARCHAR(128)') [InsuranceCompanyId]
			,@EstimateXml.value('(/VehicleDamageEstimateAddRq/AdminInfo/InsuranceCompany/Party/OrgInfo/CompanyName)[1]', 'NVARCHAR(128)') [InsuranceCompanyName]
			,@EstimateXml [RawXml]
	) x
	LEFT JOIN Service.CCCEstimates e
		ON e.RequestGuid = x.RequestGuid
	WHERE e.RequestGuid IS NULL

END