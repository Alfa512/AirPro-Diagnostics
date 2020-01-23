
CREATE PROCEDURE Access.usp_GetShopDisplayList
	@UserGuid UNIQUEIDENTIFIER
	,@AllowSelfScan BIT = NULL
	,@AllowScanAnalysis BIT = NULL
	,@HideFromReports BIT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		um.ShopGuid
		,s.DisplayName [ShopName]
	FROM Access.vwUserMemberships um
	INNER JOIN Access.Shops s
		ON um.ShopGuid = s.ShopGuid
			AND s.ActiveInd = 1
			AND (@HideFromReports IS NULL OR s.HideFromReports = @HideFromReports)
	INNER JOIN Access.Accounts a
		ON um.AccountGuid = a.AccountGuid
			AND a.ActiveInd = 1
	LEFT JOIN
	(
		SELECT
			ShopGuid
			,SUM(CASE RequestTypeId WHEN 6 THEN 1 ELSE 0 END) [AllowSelfScan]
			,SUM(CASE RequestTypeId WHEN 7 THEN 1 ELSE 0 END) [AllowScanAnalysis]
		FROM Access.ShopRequestTypes
		GROUP BY
			ShopGuid
	) srt
		ON s.ShopGuid = srt.ShopGuid
			AND (@AllowSelfScan IS NULL OR srt.AllowSelfScan = @AllowSelfScan)
			AND (@AllowScanAnalysis IS NULL OR srt.AllowScanAnalysis = @AllowScanAnalysis)
	WHERE um.UserGuid = @UserGuid
		AND ((@AllowSelfScan IS NULL AND @AllowScanAnalysis IS NULL) OR srt.ShopGuid IS NOT NULL)
	GROUP BY
		um.ShopGuid
		,s.DisplayName
	ORDER BY
		s.DisplayName

END