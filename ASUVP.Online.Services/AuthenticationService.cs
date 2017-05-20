using System;
using System.Collections.Generic;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Dto;
using ASUVP.Core.Web.Security;
using RestSharp;

namespace ASUVP.Online.Services
{
    public interface IAuthenticationService
    {
        AuthToken GetToken(string username, string password);
        UserInfoDto UserInfo(Guid companyId);
        List<CompanyDto> Companies();
    }

    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        public AuthenticationService(IEventLogger logger) : base(logger)
        {
        }

        public AuthToken GetToken(string username, string password)
        {
            var request = new RestRequest("token", Method.POST);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=utf-8;");

            request.AddParameter("grant_type", "password");
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            return Execute<AuthToken>(request);
        }

        public UserInfoDto UserInfo(Guid companyId)
        {
            var request = new RestRequest("useraccount/info", Method.GET);
            request.AddParameter("companyId", companyId);

            return Execute<UserInfoDto>(request);
        }

        public List<CompanyDto> Companies()
        {
            var request = new RestRequest("useraccount/companies", Method.GET);
            return Execute<List<CompanyDto>>(request);
        }
    }
}