using System;
using System.Text;
using System.Threading.Tasks;
using ASUVP.Core.Configuration;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Security;
using RestSharp;
using RestSharp.Deserializers;

namespace ASUVP.Online.Services
{
    public abstract class BaseHttpService
    {
        public static string Message =
            "Ошибка удаленного сервера.  Если ошибка будет повторяться, обратитесь к администратору.";

        private readonly string _endpoint;
        private readonly IEventLogger _logger;

        protected BaseHttpService(IEventLogger logger)
        {
            _logger = logger;
            _endpoint = ConfigManager.AppSetting<string>("webapi:BaseUrl");
        }

        public async Task<T> ExecuteAsync<T>(RestRequest request) where T : new()
        {
            var client = BuildClient();

            var response = await client.ExecuteTaskAsync<T>(request);

            if (response.ErrorException != null)
            {
                _logger.Error(Message, response.ErrorException);
                throw new ApplicationException(Message, response.ErrorException);
            }

            return response.Data; //todo: use json.net for deserialization
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = BuildClient();

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                _logger.Error(Message, response.ErrorException);
                throw new ApplicationException(Message, response.ErrorException);
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.Error(response.StatusDescription);
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new ApplicationException(response.StatusDescription);
            }

            return response.Data; //todo: use json.net for deserialization
        }

        private RestClient BuildClient()
        {
            var client = new RestClient(new Uri(_endpoint)) {Encoding = Encoding.UTF8};

            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            client.AddDefaultHeader("Accept", "application/json; charset=utf-8;");
            client.AddDefaultHeader("Authorization", $"Bearer {AuthManager.Token ?? string.Empty}");

            return client;
        }
    }
}