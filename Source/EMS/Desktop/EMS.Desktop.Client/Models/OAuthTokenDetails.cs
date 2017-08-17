namespace EMS.Desktop.Client.Models
{
    using Newtonsoft.Json;
    using System;

    public class OAuthTokenDetails
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty(".issued")]
        public DateTime Issued { get; set; }

        [JsonProperty(".expires")]
        public DateTime Expires { get; set; }
    }
}
