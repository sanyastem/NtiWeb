using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using ASUVP.Core.Business.Identity;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity;

namespace ASUVP.Middleware.WebApi.OAuth
{
    public class OAuthAuthorizationProvider : OAuthAuthorizationServerProvider
    {
        private readonly IEventLogger _logger;
        private readonly IdentityManager _manager;

        public OAuthAuthorizationProvider(IEventLogger logger, IdentityManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            return Task.Run(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = string.IsNullOrEmpty(context.Password)
                ? await _manager.FindByNameAsync(context.UserName)
                : await _manager.FindAsync(context.UserName, context.Password);

            // Create user here (username, createdon, createdby, isactive, isdeleted)

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.Rejected();

                LogError(context.Error, context.ErrorDescription);

                return;
            }

            if (user.IsInactive())
            {
                context.SetError("invalid_grant", "The user is inactive.");
                context.Rejected();

                LogError(context.Error, context.ErrorDescription);

                return;
            }

            var ticket = new AuthenticationTicket(CreateIdentity(context, user), AuthenticationProperties());
            context.Validated(ticket);
        }

        private ClaimsIdentity CreateIdentity(OAuthGrantResourceOwnerCredentialsContext context, User user)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            return identity;
        }

        private AuthenticationProperties AuthenticationProperties()
        {
            var dictionary = new Dictionary<string, string>
            {
                {"date_created", DateTime.Now.ToString(CultureInfo.InvariantCulture)}
            };

            return new AuthenticationProperties(dictionary);
        }

        private void LogError(string error, string description)
        {
            _logger.Error($"Access cannot be granted due to the following error: {error} - {description}");
        }
    }
}