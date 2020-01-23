CREATE PROCEDURE Reporting.usp_GetShopActivityReport
	@UserGuid UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		a.Name [AccountName]
		,a.Phone [AccountPhone]
		,m.ShopGuid
		,s.Name [ShopName]
		,s.ShopNumber
		,s.Phone [ShopPhone]
		,s.City [ShopCity]
		,st.Abbreviation + ' - ' + st.Name [ShopState]
		,m.LastRepairCreated
		,DATEDIFF(DAY, m.LastRepairCreated, GETUTCDATE()) [DaysSinceLastRepairCreated]
		,m.LastRequestCreated
		,DATEDIFF(DAY, m.LastRequestCreated, GETUTCDATE()) [DaysSinceLastRequestCreated]
		,m.LastReportCompleted
		,DATEDIFF(DAY, m.LastReportCompleted, GETUTCDATE()) [DaysSinceLastReportCreated]
		,m.LastPaymentReceived
		,DATEDIFF(DAY, m.LastPaymentReceived, GETUTCDATE()) [DaysSinceLastPaymentCreated]
		,m.LastLogin
		,DATEDIFF(DAY, m.LastLogin, GETUTCDATE()) [DaysSinceLastLastLogin]
	FROM
	(
		SELECT
			rd.ShopGuid
			,MAX(rd.RepairCreatedDt) [LastRepairCreated]
			,MAX(rd.RequestCreatedDt) [LastRequestCreated]
			,MAX(rd.ReportCompletedDt) [LastReportCompleted]
			,MAX(rd.PaymentCreatedDt) [LastPaymentReceived]
			,MAX(u.LoginAttemptDt) [LastLogin]
		FROM Reporting.ReportData rd
		LEFT JOIN
		(
			SELECT
				um.ShopGuid
				,MAX(l.LoginAttemptDt) [LoginAttemptDt]
			FROM Access.vwUserMemberships um
			INNER JOIN Access.Logins l
				ON um.UserGuid = l.UserGuid
			WHERE MembershipType IN ('Account', 'Shop')
			GROUP BY
				um.ShopGuid
		) u
			ON rd.ShopGuid = u.ShopGuid
		GROUP BY
			rd.ShopGuid
	) m
	INNER JOIN Access.Shops s
		INNER JOIN Access.Accounts a
			ON s.AccountGuid = a.AccountGuid
				AND a.ActiveInd = 1
		INNER JOIN Common.States st
			ON s.StateId = st.StateId
		ON m.ShopGuid = s.ShopGuid
			AND s.ActiveInd = 1
	WHERE m.ShopGuid IN (SELECT ShopGuid FROM Access.vwUserMemberships WHERE UserGuid = @UserGuid)

END
GO
