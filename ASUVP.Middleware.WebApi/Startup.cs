using System.Web.Http;
using log4net.Config;
using Owin;

[assembly: XmlConfigurator(Watch = true)]

namespace ASUVP.Middleware.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var container = new AutofacContainer().Build();
            XmlConfigurator.Configure();

            config.UseJsonFormatting();

            config.MapHttpAttributeRoutes();
            config.RegisterODataRoutes();
            config.RegisterApiRoutes();
            config.RegisterGlobalFillters(); // Did we need this?

            config.UseAutofacDependencyResolver(container);

            app.UseOAuth(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}