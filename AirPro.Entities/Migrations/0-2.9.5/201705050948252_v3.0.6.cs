namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v306 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Common.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        AlphaCode2 = c.String(nullable: false, maxLength: 2),
                        AlphaCode3 = c.String(nullable: false, maxLength: 3),
                        NumericCodeM49 = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "Common.States",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        Abbreviation = c.String(nullable: false, maxLength: 2),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("Common.Countries", t => t.CountryId)
                .Index(t => t.CountryId);

            string countriesSQL = @"INSERT INTO Common.Countries (AlphaCode2, AlphaCode3, NumericCodeM49, Name)
		        SELECT 'US' [AlphaCode2], 'USA' [AlphaCode3], '840' [NumericCodeM49], 'United States of America' [Name] UNION
		        SELECT 'CA' [AlphaCode2], 'CAN' [AlphaCode3], '124' [NumericCodeM49], 'Canada' [Name]";
            Sql(countriesSQL);

            string statesSQL = @"INSERT INTO Common.States (CountryId, Abbreviation, Name)
		        SELECT
			        (SELECT CountryId FROM Common.Countries WHERE AlphaCode2 = 'US') [CountryId]
			        ,Abbreviation
			        ,Name
		        FROM
		        (
			        SELECT 'AL' [Abbreviation], 'Alabama' [Name] UNION
			        SELECT 'AK' [Abbreviation], 'Alaska' [Name] UNION
                    SELECT 'AS' [Abbreviation], 'American Samoa' [Name] UNION
			        SELECT 'AZ' [Abbreviation], 'Arizona' [Name] UNION
			        SELECT 'AR' [Abbreviation], 'Arkansas' [Name] UNION
                    SELECT 'AA' [Abbreviation], 'Armed Forces Americas' [Name] UNION
                    SELECT 'AE' [Abbreviation], 'Armed Forces Europe' [Name] UNION
                    SELECT 'AP' [Abbreviation], 'Armed Forces Pacific' [Name] UNION
			        SELECT 'CA' [Abbreviation], 'California' [Name] UNION
			        SELECT 'CO' [Abbreviation], 'Colorado' [Name] UNION
			        SELECT 'CT' [Abbreviation], 'Connecticut' [Name] UNION
                    SELECT 'DC' [Abbreviation], 'District of Columbia' [Name] UNION
			        SELECT 'DE' [Abbreviation], 'Delaware' [Name] UNION
                    SELECT 'FM' [Abbreviation], 'Federated States of Micronesia' [Name] UNION
			        SELECT 'FL' [Abbreviation], 'Florida' [Name] UNION
			        SELECT 'GA' [Abbreviation], 'Georgia' [Name] UNION
                    SELECT 'GU' [Abbreviation], 'Guam' [Name] UNION
			        SELECT 'HI' [Abbreviation], 'Hawaii' [Name] UNION
			        SELECT 'ID' [Abbreviation], 'Idaho' [Name] UNION
			        SELECT 'IL' [Abbreviation], 'Illinois' [Name] UNION
			        SELECT 'IN' [Abbreviation], 'Indiana' [Name] UNION
			        SELECT 'IA' [Abbreviation], 'Iowa' [Name] UNION
			        SELECT 'KS' [Abbreviation], 'Kansas' [Name] UNION
			        SELECT 'KY' [Abbreviation], 'Kentucky' [Name] UNION
			        SELECT 'LA' [Abbreviation], 'Louisiana' [Name] UNION
			        SELECT 'ME' [Abbreviation], 'Maine' [Name] UNION
                    SELECT 'MH' [Abbreviation], 'Marshall Islands' [Name] UNION
			        SELECT 'MD' [Abbreviation], 'Maryland' [Name] UNION
			        SELECT 'MA' [Abbreviation], 'Massachusetts' [Name] UNION
			        SELECT 'MI' [Abbreviation], 'Michigan' [Name] UNION
			        SELECT 'MN' [Abbreviation], 'Minnesota' [Name] UNION
			        SELECT 'MS' [Abbreviation], 'Mississippi' [Name] UNION
			        SELECT 'MO' [Abbreviation], 'Missouri' [Name] UNION
			        SELECT 'MT' [Abbreviation], 'Montana' [Name] UNION
			        SELECT 'NE' [Abbreviation], 'Nebraska' [Name] UNION
			        SELECT 'NV' [Abbreviation], 'Nevada' [Name] UNION
			        SELECT 'NH' [Abbreviation], 'New Hampshire' [Name] UNION
			        SELECT 'NJ' [Abbreviation], 'New Jersey' [Name] UNION
			        SELECT 'NM' [Abbreviation], 'New Mexico' [Name] UNION
			        SELECT 'NY' [Abbreviation], 'New York' [Name] UNION
			        SELECT 'NC' [Abbreviation], 'North Carolina' [Name] UNION
			        SELECT 'ND' [Abbreviation], 'North Dakota' [Name] UNION
                    SELECT 'MP' [Abbreviation], 'Northern Mariana Islands' [Name] UNION
			        SELECT 'OH' [Abbreviation], 'Ohio' [Name] UNION
			        SELECT 'OK' [Abbreviation], 'Oklahoma' [Name] UNION
			        SELECT 'OR' [Abbreviation], 'Oregon' [Name] UNION
                    SELECT 'PW' [Abbreviation], 'Palau' [Name] UNION
			        SELECT 'PA' [Abbreviation], 'Pennsylvania' [Name] UNION
                    SELECT 'PR' [Abbreviation], 'Puerto Rico' [Name] UNION
			        SELECT 'RI' [Abbreviation], 'Rhode Island' [Name] UNION
			        SELECT 'SC' [Abbreviation], 'South Carolina' [Name] UNION
			        SELECT 'SD' [Abbreviation], 'South Dakota' [Name] UNION
			        SELECT 'TN' [Abbreviation], 'Tennessee' [Name] UNION
			        SELECT 'TX' [Abbreviation], 'Texas' [Name] UNION
			        SELECT 'UT' [Abbreviation], 'Utah' [Name] UNION
			        SELECT 'VT' [Abbreviation], 'Vermont' [Name] UNION
			        SELECT 'VA' [Abbreviation], 'Virginia' [Name] UNION
                    SELECT 'VI' [Abbreviation], 'Virgin Islands' [Name] UNION
			        SELECT 'WA' [Abbreviation], 'Washington' [Name] UNION
			        SELECT 'WV' [Abbreviation], 'West Virginia' [Name] UNION
			        SELECT 'WI' [Abbreviation], 'Wisconsin' [Name] UNION
			        SELECT 'WY' [Abbreviation], 'Wyoming' [Name]
		        ) us

		        UNION 

		        SELECT
			        (SELECT CountryId FROM Common.Countries WHERE AlphaCode2 = 'CA') [CountryId]
			        ,Abbreviation
			        ,Name
		        FROM
		        (
			        SELECT 'AB' [Abbreviation], 'Alberta' [Name] UNION
			        SELECT 'BC' [Abbreviation], 'British Columbia' [Name] UNION
			        SELECT 'LB' [Abbreviation], 'Labrador' [Name] UNION
			        SELECT 'MB' [Abbreviation], 'Manitoba' [Name] UNION
			        SELECT 'NB' [Abbreviation], 'New Brunswick' [Name] UNION
			        SELECT 'NF' [Abbreviation], 'Newfoundland' [Name] UNION
			        SELECT 'NS' [Abbreviation], 'Nova Scotia' [Name] UNION
			        SELECT 'NU' [Abbreviation], 'Nunavut' [Name] UNION
			        SELECT 'NW' [Abbreviation], 'Northwest Territories' [Name] UNION
			        SELECT 'ON' [Abbreviation], 'Ontario' [Name] UNION
			        SELECT 'PE' [Abbreviation], 'Prince Edward Island' [Name] UNION
			        SELECT 'QC' [Abbreviation], 'Quebec' [Name] UNION
			        SELECT 'SK' [Abbreviation], 'Saskatchewen' [Name] UNION
			        SELECT 'YU' [Abbreviation], 'Yukon' [Name]
		        ) ca";
            Sql(statesSQL);

            AddColumn("Scan.Reports", "ReportVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("Access.Accounts", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("Common.States", "CountryId", "Common.Countries");
            DropIndex("Common.States", new[] { "CountryId" });
            AlterColumn("Access.Accounts", "Name", c => c.String());
            DropColumn("Scan.Reports", "ReportVersion");
            DropTable("Common.States");
            DropTable("Common.Countries");
        }
    }
}
