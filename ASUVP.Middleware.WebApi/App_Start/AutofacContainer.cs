using ASUVP.Core.Business;
using ASUVP.Core.DataAccess;
using ASUVP.Core.Logging;
using Autofac;
using Autofac.Integration.WebApi;

namespace ASUVP.Middleware.WebApi
{
    public class AutofacContainer
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof (AutofacContainer).Assembly);

            builder.RegisterType<EventLogger>()
                .As<IEventLogger>()
                .WithParameter("name", "WebApiEventLogger")
                .SingleInstance();

            builder.RegisterModule(new AutofacBusinessModule());
            builder.RegisterModule(new AutofacDataAccessModule());
            builder.RegisterModule(new AutofacMiddlewareModule());

            return builder.Build();
        }
    }
}