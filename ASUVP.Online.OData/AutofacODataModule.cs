using Autofac;

namespace ASUVP.Online.OData
{
    public class AutofacODataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ODataClient>().AsImplementedInterfaces();
        }
    }
}