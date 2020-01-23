using System.Text;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v280 : DbMigration
    {
        public override void Up()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE t");
            sql.AppendLine("SET t.Options = t.Options + ',ShopName'");
            sql.AppendLine("FROM [Support].[NotificationTemplates] t");
            sql.AppendLine("WHERE t.Name IN ('ShopReportEmail', 'ShopInvoiceEmail')");
            sql.AppendLine("AND t.Options NOT LIKE '%ShopName%'");
            sql.AppendLine("GO");

            Sql(sql.ToString());
        }
        
        public override void Down()
        {
        }
    }
}
