using Newtonsoft.Json;

namespace ASUVP.Core.Web.Security
{
    public class AuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(AccessToken);
        }
    }
}