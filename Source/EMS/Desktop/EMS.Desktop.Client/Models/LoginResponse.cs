namespace EMS.Desktop.Client.Models
{
    using Newtonsoft.Json;

    public class LoginResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
