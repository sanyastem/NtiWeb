using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;

namespace ASUVP.Core.DataAccess.Context
{
    public class DataContext : DbContext
    {
        public DataContext() : base(nameof(DataContext))
        {
            DbConfiguration.SetConfiguration(new DataConfiguration());
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            builder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            builder.Configurations.AddFromAssembly(GetType().Assembly);
        }
    }

    public class DataConfiguration : DbConfiguration
    {
        public DataConfiguration()
        {
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            SetDatabaseInitializer(new CreateDatabaseIfNotExists<DataContext>());
        }
    }
}