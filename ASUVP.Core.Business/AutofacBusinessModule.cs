using ASUVP.Core.Business.Identity;
using Autofac;

namespace ASUVP.Core.Business
{
    public class AutofacBusinessModule : Module
    {
        private const string Manager = "Manager";

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = GetType().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith(Manager))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<IdentityManager>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}