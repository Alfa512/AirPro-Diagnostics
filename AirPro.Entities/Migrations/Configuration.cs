using System.Data.Entity.Migrations;
using UniMatrix.Common.Entity;

namespace AirPro.Entities.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<EntityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new DropObjectSqlServerMigrationSqlGenerator());
        }

        protected override void Seed(EntityDbContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

            References.SeedAccess(ref context);

            References.SeedNotificationTypes(ref context);

            References.SeedPointOfImpacts(ref context);

            References.SeedDiagnosticTools(ref context);

            References.SeedDiagnosticUploadFileTypes(ref context);

            References.SeedUploadTypes(ref context);

            References.SeedRepairStatuses(ref context);

            References.SeedNoteTypes(ref context);

            References.SeedDefaults(ref context);

            References.SeedLookups(ref context);

            References.CreateViews(ref context);

            References.SeedUserDefinedTypes(ref context);

            References.CreateProcedures(ref context);

            References.CreateTriggers(ref context);
        }
    }
}
