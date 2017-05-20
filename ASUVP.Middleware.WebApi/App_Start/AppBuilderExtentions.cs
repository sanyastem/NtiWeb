using System;
using ASUVP.Middleware.WebApi.OAuth;
using Autofac;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace ASUVP.Middleware.WebApi
{
    public static class AppBuilderExtentions
    {
        public static void UseOAuth(this IAppBuilder app, IContainer container)
        {
            var options = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(24),
                Provider = container.Resolve<OAuthAuthorizationProvider>(),
                AllowInsecureHttp = true
            };

            app.UseOAuthBearerTokens(options);
        }
    }
}