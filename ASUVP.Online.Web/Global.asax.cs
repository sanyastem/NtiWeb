using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using DevExpress.Web.Mvc;
using log4net.Config;
using System.Globalization;
using System.Threading;
using System.IO.Compression;

[assembly: XmlConfigurator(Watch = true)]

namespace ASUVP.Online.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.Configure();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.Configure(GlobalFilters.Filters);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            ModelBinders.Binders.DefaultBinder = new DevExpressEditorsBinder();

            var container = new AutofacContainer().Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            RemoveWebFormsEngine();
        }
       /* protected void Application_BeginRequest(object sender, EventArgs e)
        {

            // Implement HTTP compression
            HttpApplication app = (HttpApplication)sender;


            // Retrieve accepted encodings
            string encodings = app.Request.Headers.Get("Accept-Encoding");
            if (encodings != null)
            {
                // Check the browser accepts deflate or gzip (deflate takes preference)
                encodings = encodings.ToLower();
                if (encodings.Contains("deflate"))
                {
                    app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
                    app.Response.AppendHeader("Content-Encoding", "deflate");
                }
                else if (encodings.Contains("gzip"))
                {
                    app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
                    app.Response.AppendHeader("Content-Encoding", "gzip");
                }
            }
        }*/
        private static void RemoveWebFormsEngine()
        {
            var engines = ViewEngines.Engines;

            var webFormsEngine = engines.OfType<WebFormViewEngine>().FirstOrDefault();
            if (webFormsEngine != null)
            {
                engines.Remove(webFormsEngine);
            }
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            /*HttpApplication application = (HttpApplication)sender;

            if (application.Response.StatusCode != 401 || !application.Request.IsAuthenticated) return;

            application.Response.ClearContent();
            Server.Transfer("~/Account/Login");
            HttpContext.Current.Server.ClearError();*/
        }

        protected void Application_PreRequestHandlerExecute()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");
        }
    }
}