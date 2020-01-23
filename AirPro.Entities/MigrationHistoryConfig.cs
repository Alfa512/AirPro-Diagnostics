using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace AirPro.Site
{
    public class MigrationHistoryConfig : HistoryContext
    {
        public MigrationHistoryConfig(DbConnection dbConnection, string defaultSchema)
            :base (dbConnection, defaultSchema)
        {
            Database.SetInitializer<MigrationHistoryConfig>(new DropCreateDatabaseAlways<MigrationHistoryConfig>());
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoryRow>().ToTable(tableName: "MigrationHistory", schemaName: "Support");
        }
    }

    public class ModelConfiguration : DbConfiguration
    {
        public ModelConfiguration()
        {
            this.SetHistoryContext("System.Data.SqlClient",
                (connection, defaultSchema) => new MigrationHistoryConfig(connection, defaultSchema));
        }
    } 
}