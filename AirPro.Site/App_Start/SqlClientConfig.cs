using System.Configuration;
using System.Data.Entity.Migrations;

namespace AirPro.Site
{
    public class SqlClientConfig
    {
        public static void EnableCodeFirstMigrations()
        {
            // Check For Update.
            if (!bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"])) return;

            // Update MVC Application.
            var appConfig = new AirPro.Entities.Migrations.Configuration();
            var appMigrator = new DbMigrator(appConfig);
            appMigrator.Update();
        }
    }
}
