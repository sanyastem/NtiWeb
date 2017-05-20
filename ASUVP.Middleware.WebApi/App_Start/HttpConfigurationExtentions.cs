using System;
using System.Web.Http;
using System.Web.OData.Extensions;
using Autofac;
using Autofac.Integration.WebApi;

namespace ASUVP.Middleware.WebApi
{
    public static class HttpConfigurationExtentions
    {
        public static void RegisterODataRoutes(this HttpConfiguration config)
        {
            config.SetTimeZoneInfo(TimeZoneInfo.Utc);

            config.AddODataQueryFilter();
            config.EnableUnqualifiedNameCall(true);
            config.EnableEnumPrefixFree(true);

            config.MapODataServiceRoute("odata", "odata", EdmModelBuilder.Build());
        }

        public static void RegisterApiRoutes(this HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("api", "api/{controller}/{id}", new {id = RouteParameter.Optional});
        }

        public static void UseAutofacDependencyResolver(this HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public static void UseJsonFormatting(this HttpConfiguration config)
        {
            config.Formatters.XmlFormatter.MediaTypeMappings.Clear();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }

        public static void RegisterGlobalFillters(this HttpConfiguration config)
        {
            config.Filters.Add(new HostAuthenticationFilter("Bearer"));
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}