using System;
using System.Threading.Tasks;
using ASUVP.Core.Web.Security;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ASUVP.Online.Web.Startup))]

namespace ASUVP.Online.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var idProvider = new CustomUserIdProvider();

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            return AuthManager.User.UserId.ToString();
        }
    }
}
