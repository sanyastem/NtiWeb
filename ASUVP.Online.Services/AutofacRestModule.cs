using Autofac;

namespace ASUVP.Online.Services
{
    public class AutofacRestModule : Module
    {
        private const string ODataService = "Service";

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = GetType().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith(ODataService))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}