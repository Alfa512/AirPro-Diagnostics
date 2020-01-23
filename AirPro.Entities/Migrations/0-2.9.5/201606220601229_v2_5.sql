

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Repair' AND  TABLE_NAME = 'InsuranceCompanies')
	AND (SELECT COUNT(1) FROM Repair.InsuranceCompanies) = 1
BEGIN

	INSERT INTO Repair.InsuranceCompanies (InsuranceCompanyName)
	SELECT i.InsuranceCompany
	FROM
	(
		SELECT 'Self Pay' [InsuranceCompany] UNION ALL
		SELECT '21st Century Insurance' UNION ALL
		SELECT 'Acuity' UNION ALL
		SELECT 'ACE Limited' UNION ALL
		SELECT 'Aetna' UNION ALL
		SELECT 'Aflac' UNION ALL
		SELECT 'Alleghany Corporation' UNION ALL
		SELECT 'Allied Insurance' UNION ALL
		SELECT 'Allstate' UNION ALL
		SELECT 'American Automobile Association' UNION ALL
		SELECT 'American Family Insurance' UNION ALL
		SELECT 'American Income Life Insurance Company' UNION ALL
		SELECT 'American International Group (AIG)' UNION ALL
		SELECT 'American National Insurance Company' UNION ALL
		SELECT 'Ameritas Life Insurance Company' UNION ALL
		SELECT 'Amica Mutual Insurance' UNION ALL
		SELECT 'Applied Underwriters' UNION ALL
		SELECT 'Arbella Insurance Group' UNION ALL
		SELECT 'Assurant' UNION ALL
		SELECT 'Assurity Life Insurance Company' UNION ALL
		SELECT 'Auto-Owners Insurance' UNION ALL
		SELECT 'AXA Equitable Life Insurance Company' UNION ALL
		SELECT 'Bankers Life and Casualty Company' UNION ALL
		SELECT 'Berkshire Hathaway' UNION ALL
		SELECT 'California Casualty' UNION ALL
		SELECT 'Cincinnati Insurance Company' UNION ALL
		SELECT 'CNA Financial' UNION ALL
		SELECT 'Colonial Life & Accident Insurance Company' UNION ALL
		SELECT 'Combined Insurance' UNION ALL
		SELECT 'Commerce Insurance Group' UNION ALL
		SELECT 'Conseco' UNION ALL
		SELECT 'Country Financial' UNION ALL
		SELECT 'Chartis' UNION ALL
		SELECT 'Chubb Corp.' UNION ALL
		SELECT 'Elephant.com' UNION ALL
		SELECT 'Encompass Insurance Company' UNION ALL
		SELECT 'Erie Insurance Group' UNION ALL
		SELECT 'Esurance' UNION ALL
		SELECT 'Evergreen USA RRG' UNION ALL
		SELECT 'FM Global' UNION ALL
		SELECT 'Family Heritage' UNION ALL
		SELECT 'Farmers Insurance Group' UNION ALL
		SELECT 'Federated Mutual Insurance Company' UNION ALL
		SELECT 'First Catholic Slovak Ladies Association of the United States of America' UNION ALL
		SELECT 'FirstComp Insurance Company' UNION ALL
		SELECT 'First Insurance Company of Hawaii' UNION ALL
		SELECT 'GAINSCO' UNION ALL
		SELECT 'GEICO' UNION ALL
		SELECT 'General Re' UNION ALL
		SELECT 'Genworth Financial' UNION ALL
		SELECT 'GMAC Insurance' UNION ALL
		SELECT 'Gracy Title Company' UNION ALL
		SELECT 'Grange Mutual Casualty Company' UNION ALL
		SELECT 'Guarantee Insurance Company' UNION ALL
		SELECT 'Guardian Life Insurance Company of America' UNION ALL
		SELECT 'GuideOne Insurance' UNION ALL
		SELECT 'Hanover Insurance' UNION ALL
		SELECT 'The Hartford' UNION ALL
		SELECT 'HCC Insurance Holdings' UNION ALL
		SELECT 'Horace Mann Insurance Company' UNION ALL
		SELECT 'Infinity Property & Casualty Corporation' UNION ALL
		SELECT 'IntelliQuote Insurance Services' UNION ALL
		SELECT 'Jackson National Life' UNION ALL
		SELECT 'John Hancock Insurance' UNION ALL
		SELECT 'K&K Insurance' UNION ALL
		SELECT 'Kansas City Life Insurance Company' UNION ALL
		SELECT 'Kentucky Farm Bureau' UNION ALL
		SELECT 'Knights of Columbus' UNION ALL
		SELECT 'Liberty Mutual' UNION ALL
		SELECT 'Lincoln National Corporation' UNION ALL
		SELECT 'Markel Corporation' UNION ALL
		SELECT 'MassMutual Financial Group' UNION ALL
		SELECT 'Merchants Insurance Group' UNION ALL
		SELECT 'Mercury Insurance Group' UNION ALL
		SELECT 'MetLife' UNION ALL
		SELECT 'Mutual of Omaha' UNION ALL
		SELECT 'National Life' UNION ALL
		SELECT 'Nationwide Mutual Insurance Company' UNION ALL
		SELECT 'New Jersey Manufacturers Insurance Company' UNION ALL
		SELECT 'New York Life Insurance Company' UNION ALL
		SELECT 'Nonprofits Insurance Alliance Group' UNION ALL
		SELECT 'Northwestern Mutual' UNION ALL
		SELECT 'Ocean Harbor Casualty Insurance Company' UNION ALL
		SELECT 'Ohio Mutual Insurance Group' UNION ALL
		SELECT 'Omega' UNION ALL
		SELECT 'OneBeacon' UNION ALL
		SELECT 'Oscar' UNION ALL
		SELECT 'Oxford Health Plans' UNION ALL
		SELECT 'Pacific Life' UNION ALL
		SELECT 'Pacificare' UNION ALL
		SELECT 'PEMCO' UNION ALL
		SELECT 'Penn Mutual' UNION ALL
		SELECT 'Philadelphia Contributionship for the Insurance of Houses from Loss by Fire' UNION ALL
		SELECT 'Philadelphia Insurance Companies' UNION ALL
		SELECT 'Physicians Mutual' UNION ALL
		SELECT 'Principal Financial Group' UNION ALL
		SELECT 'Primerica' UNION ALL
		SELECT 'Progressive' UNION ALL
		SELECT 'Protective Life' UNION ALL
		SELECT 'Prudential Financial' UNION ALL
		SELECT 'QBE' UNION ALL
		SELECT 'The Regence Group' UNION ALL
		SELECT 'Reliance Insurance Company' UNION ALL
		SELECT 'RLI Corp.' UNION ALL
		SELECT 'Safe Auto Insurance Company' UNION ALL
		SELECT 'Safeco' UNION ALL
		SELECT 'Safeway Insurance Group' UNION ALL
		SELECT 'Secura Insurance Company' UNION ALL
		SELECT 'Sentry Insurance' UNION ALL
		SELECT 'Selective Insurance' UNION ALL
		SELECT 'Shelter Insurance' UNION ALL
		SELECT 'Southern Aid and Insurance Company' UNION ALL
		SELECT 'Standard Insurance Company' UNION ALL
		SELECT 'State Auto Insurance Group' UNION ALL
		SELECT 'State Farm Insurance' UNION ALL
		SELECT 'Sun Life Financial' UNION ALL
		SELECT 'Symetra' UNION ALL
		SELECT 'TIAA-CREF' UNION ALL
		SELECT 'The Main Street America Group' UNION ALL
		SELECT 'The Norfolk & Dedham Group' UNION ALL
		SELECT 'The Travelers Companies' UNION ALL
		SELECT 'Trupanion' UNION ALL
		SELECT 'Unitrin Direct Auto Insurance' UNION ALL
		SELECT 'Unum' UNION ALL
		SELECT 'USAA' UNION ALL
		SELECT 'West Bend' UNION ALL
		SELECT 'West Coast Life' UNION ALL
		SELECT 'Western Mutual Insurance Group' UNION ALL
		SELECT 'Western & Southern Financial Group' UNION ALL
		SELECT 'Westfield Insurance' UNION ALL
		SELECT 'White Mountains Insurance Group' UNION ALL
		SELECT 'Workmens Auto Insurance Company' UNION ALL
		SELECT 'Zurich NA'
	) i

END