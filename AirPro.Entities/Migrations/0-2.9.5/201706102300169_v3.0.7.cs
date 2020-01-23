namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v307 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF (OBJECT_ID('tempdb..#Translate') IS NOT NULL) DROP TABLE #Translate
                SELECT StateId, EnumId
                INTO #Translate
                FROM Common.States s
                INNER JOIN
                (
	                SELECT 0 [EnumId], 'AL' [EnumAbbr] UNION
	                SELECT 1 [EnumId], 'AK' [EnumAbbr] UNION
	                SELECT 2 [EnumId], 'AR' [EnumAbbr] UNION
	                SELECT 3 [EnumId], 'AZ' [EnumAbbr] UNION
	                SELECT 4 [EnumId], 'CA' [EnumAbbr] UNION
	                SELECT 5 [EnumId], 'CO' [EnumAbbr] UNION
	                SELECT 6 [EnumId], 'CT' [EnumAbbr] UNION
	                SELECT 7 [EnumId], 'DC' [EnumAbbr] UNION
	                SELECT 8 [EnumId], 'DE' [EnumAbbr] UNION
	                SELECT 9 [EnumId], 'FL' [EnumAbbr] UNION
	                SELECT 10 [EnumId], 'GA' [EnumAbbr] UNION
	                SELECT 11 [EnumId], 'HI' [EnumAbbr] UNION
	                SELECT 12 [EnumId], 'IA' [EnumAbbr] UNION
	                SELECT 13 [EnumId], 'ID' [EnumAbbr] UNION
	                SELECT 14 [EnumId], 'IL' [EnumAbbr] UNION
	                SELECT 15 [EnumId], 'IN' [EnumAbbr] UNION
	                SELECT 16 [EnumId], 'KS' [EnumAbbr] UNION
	                SELECT 17 [EnumId], 'KY' [EnumAbbr] UNION
	                SELECT 18 [EnumId], 'LA' [EnumAbbr] UNION
	                SELECT 19 [EnumId], 'MA' [EnumAbbr] UNION
	                SELECT 20 [EnumId], 'MD' [EnumAbbr] UNION
	                SELECT 21 [EnumId], 'ME' [EnumAbbr] UNION
	                SELECT 22 [EnumId], 'MI' [EnumAbbr] UNION
	                SELECT 23 [EnumId], 'MN' [EnumAbbr] UNION
	                SELECT 24 [EnumId], 'MO' [EnumAbbr] UNION
	                SELECT 25 [EnumId], 'MS' [EnumAbbr] UNION
	                SELECT 26 [EnumId], 'MT' [EnumAbbr] UNION
	                SELECT 27 [EnumId], 'NC' [EnumAbbr] UNION
	                SELECT 28 [EnumId], 'ND' [EnumAbbr] UNION
	                SELECT 29 [EnumId], 'NE' [EnumAbbr] UNION
	                SELECT 30 [EnumId], 'NH' [EnumAbbr] UNION
	                SELECT 31 [EnumId], 'NJ' [EnumAbbr] UNION
	                SELECT 32 [EnumId], 'NM' [EnumAbbr] UNION
	                SELECT 33 [EnumId], 'NV' [EnumAbbr] UNION
	                SELECT 34 [EnumId], 'NY' [EnumAbbr] UNION
	                SELECT 35 [EnumId], 'OK' [EnumAbbr] UNION
	                SELECT 36 [EnumId], 'OH' [EnumAbbr] UNION
	                SELECT 37 [EnumId], 'OR' [EnumAbbr] UNION
	                SELECT 38 [EnumId], 'PA' [EnumAbbr] UNION
	                SELECT 39 [EnumId], 'RI' [EnumAbbr] UNION
	                SELECT 40 [EnumId], 'SC' [EnumAbbr] UNION
	                SELECT 41 [EnumId], 'SD' [EnumAbbr] UNION
	                SELECT 42 [EnumId], 'TN' [EnumAbbr] UNION
	                SELECT 43 [EnumId], 'TX' [EnumAbbr] UNION
	                SELECT 44 [EnumId], 'UT' [EnumAbbr] UNION
	                SELECT 45 [EnumId], 'VA' [EnumAbbr] UNION
	                SELECT 46 [EnumId], 'VT' [EnumAbbr] UNION
	                SELECT 47 [EnumId], 'WA' [EnumAbbr] UNION
	                SELECT 48 [EnumId], 'WI' [EnumAbbr] UNION
	                SELECT 49 [EnumId], 'WV' [EnumAbbr] UNION
	                SELECT 50 [EnumId], 'WY' [EnumAbbr] UNION
	                SELECT 51 [EnumId], 'AB' [EnumAbbr] UNION
	                SELECT 52 [EnumId], 'BC' [EnumAbbr] UNION
	                SELECT 53 [EnumId], 'MB' [EnumAbbr] UNION
	                SELECT 54 [EnumId], 'NB' [EnumAbbr] UNION
	                SELECT 55 [EnumId], 'NL' [EnumAbbr] UNION
	                SELECT 56 [EnumId], 'NT' [EnumAbbr] UNION
	                SELECT 57 [EnumId], 'NS' [EnumAbbr] UNION
	                SELECT 58 [EnumId], 'NU' [EnumAbbr] UNION
	                SELECT 59 [EnumId], 'ON' [EnumAbbr] UNION
	                SELECT 60 [EnumId], 'PE' [EnumAbbr] UNION
	                SELECT 61 [EnumId], 'QC' [EnumAbbr] UNION
	                SELECT 62 [EnumId], 'SK' [EnumAbbr] UNION
	                SELECT 63 [EnumId], 'YT' [EnumAbbr]
                ) o
	                ON s.Abbreviation = o.EnumAbbr

                UPDATE a
	                SET a.State = t.StateId
                FROM Access.Accounts a
                INNER JOIN #Translate t
	                ON a.State = t.EnumId

                UPDATE s
	                SET s.State = t.StateId
                FROM Access.Shops s
                INNER JOIN #Translate t
	                ON s.State = t.EnumId

                DROP TABLE #Translate");

            RenameColumn("Access.Accounts", "State", "StateId");
            RenameColumn("Access.Shops", "State", "StateId");
            AlterColumn("Access.Accounts", "StateId", c => c.Int(nullable: false));
            AlterColumn("Access.Shops", "StateId", c => c.Int(nullable: false));
            CreateIndex("Access.Accounts", "StateId");
            CreateIndex("Access.Shops", "StateId");
            AddForeignKey("Access.Shops", "StateId", "Common.States", "StateId");
            AddForeignKey("Access.Accounts", "StateId", "Common.States", "StateId");
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Accounts", "StateId", "Common.States");
            DropForeignKey("Access.Shops", "StateId", "Common.States");
            DropIndex("Access.Shops", new[] { "StateId" });
            DropIndex("Access.Accounts", new[] { "StateId" });
            RenameColumn("Access.Accounts", "StateId", "State");
            RenameColumn("Access.Shops", "StateId", "State");

            Sql(@"IF (OBJECT_ID('tempdb..#Translate') IS NOT NULL) DROP TABLE #Translate
                SELECT StateId, EnumId
                INTO #Translate
                FROM Common.States s
                INNER JOIN
                (
	                SELECT 0 [EnumId], 'AL' [EnumAbbr] UNION
	                SELECT 1 [EnumId], 'AK' [EnumAbbr] UNION
	                SELECT 2 [EnumId], 'AR' [EnumAbbr] UNION
	                SELECT 3 [EnumId], 'AZ' [EnumAbbr] UNION
	                SELECT 4 [EnumId], 'CA' [EnumAbbr] UNION
	                SELECT 5 [EnumId], 'CO' [EnumAbbr] UNION
	                SELECT 6 [EnumId], 'CT' [EnumAbbr] UNION
	                SELECT 7 [EnumId], 'DC' [EnumAbbr] UNION
	                SELECT 8 [EnumId], 'DE' [EnumAbbr] UNION
	                SELECT 9 [EnumId], 'FL' [EnumAbbr] UNION
	                SELECT 10 [EnumId], 'GA' [EnumAbbr] UNION
	                SELECT 11 [EnumId], 'HI' [EnumAbbr] UNION
	                SELECT 12 [EnumId], 'IA' [EnumAbbr] UNION
	                SELECT 13 [EnumId], 'ID' [EnumAbbr] UNION
	                SELECT 14 [EnumId], 'IL' [EnumAbbr] UNION
	                SELECT 15 [EnumId], 'IN' [EnumAbbr] UNION
	                SELECT 16 [EnumId], 'KS' [EnumAbbr] UNION
	                SELECT 17 [EnumId], 'KY' [EnumAbbr] UNION
	                SELECT 18 [EnumId], 'LA' [EnumAbbr] UNION
	                SELECT 19 [EnumId], 'MA' [EnumAbbr] UNION
	                SELECT 20 [EnumId], 'MD' [EnumAbbr] UNION
	                SELECT 21 [EnumId], 'ME' [EnumAbbr] UNION
	                SELECT 22 [EnumId], 'MI' [EnumAbbr] UNION
	                SELECT 23 [EnumId], 'MN' [EnumAbbr] UNION
	                SELECT 24 [EnumId], 'MO' [EnumAbbr] UNION
	                SELECT 25 [EnumId], 'MS' [EnumAbbr] UNION
	                SELECT 26 [EnumId], 'MT' [EnumAbbr] UNION
	                SELECT 27 [EnumId], 'NC' [EnumAbbr] UNION
	                SELECT 28 [EnumId], 'ND' [EnumAbbr] UNION
	                SELECT 29 [EnumId], 'NE' [EnumAbbr] UNION
	                SELECT 30 [EnumId], 'NH' [EnumAbbr] UNION
	                SELECT 31 [EnumId], 'NJ' [EnumAbbr] UNION
	                SELECT 32 [EnumId], 'NM' [EnumAbbr] UNION
	                SELECT 33 [EnumId], 'NV' [EnumAbbr] UNION
	                SELECT 34 [EnumId], 'NY' [EnumAbbr] UNION
	                SELECT 35 [EnumId], 'OK' [EnumAbbr] UNION
	                SELECT 36 [EnumId], 'OH' [EnumAbbr] UNION
	                SELECT 37 [EnumId], 'OR' [EnumAbbr] UNION
	                SELECT 38 [EnumId], 'PA' [EnumAbbr] UNION
	                SELECT 39 [EnumId], 'RI' [EnumAbbr] UNION
	                SELECT 40 [EnumId], 'SC' [EnumAbbr] UNION
	                SELECT 41 [EnumId], 'SD' [EnumAbbr] UNION
	                SELECT 42 [EnumId], 'TN' [EnumAbbr] UNION
	                SELECT 43 [EnumId], 'TX' [EnumAbbr] UNION
	                SELECT 44 [EnumId], 'UT' [EnumAbbr] UNION
	                SELECT 45 [EnumId], 'VA' [EnumAbbr] UNION
	                SELECT 46 [EnumId], 'VT' [EnumAbbr] UNION
	                SELECT 47 [EnumId], 'WA' [EnumAbbr] UNION
	                SELECT 48 [EnumId], 'WI' [EnumAbbr] UNION
	                SELECT 49 [EnumId], 'WV' [EnumAbbr] UNION
	                SELECT 50 [EnumId], 'WY' [EnumAbbr] UNION
	                SELECT 51 [EnumId], 'AB' [EnumAbbr] UNION
	                SELECT 52 [EnumId], 'BC' [EnumAbbr] UNION
	                SELECT 53 [EnumId], 'MB' [EnumAbbr] UNION
	                SELECT 54 [EnumId], 'NB' [EnumAbbr] UNION
	                SELECT 55 [EnumId], 'NL' [EnumAbbr] UNION
	                SELECT 56 [EnumId], 'NT' [EnumAbbr] UNION
	                SELECT 57 [EnumId], 'NS' [EnumAbbr] UNION
	                SELECT 58 [EnumId], 'NU' [EnumAbbr] UNION
	                SELECT 59 [EnumId], 'ON' [EnumAbbr] UNION
	                SELECT 60 [EnumId], 'PE' [EnumAbbr] UNION
	                SELECT 61 [EnumId], 'QC' [EnumAbbr] UNION
	                SELECT 62 [EnumId], 'SK' [EnumAbbr] UNION
	                SELECT 63 [EnumId], 'YT' [EnumAbbr]
                ) o
	                ON s.Abbreviation = o.EnumAbbr

                UPDATE a
	                SET a.State = t.EnumId
                FROM Access.Accounts a
                INNER JOIN #Translate t
	                ON a.State = t.StateId

                UPDATE s
	                SET s.State = t.EnumId
                FROM Access.Shops s
                INNER JOIN #Translate t
	                ON s.State = t.StateId

                DROP TABLE #Translate");
        }
    }
}
