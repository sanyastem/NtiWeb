using System.Data.Entity;
using ASUVP.Core.DataAccess.Context;
using Autofac;

namespace ASUVP.Core.DataAccess
{
    public class AutofacDataAccessModule : Module
    {
        private const string Repository = "Repository";

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = GetType().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith(Repository))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<DataContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();
        }
    }
}