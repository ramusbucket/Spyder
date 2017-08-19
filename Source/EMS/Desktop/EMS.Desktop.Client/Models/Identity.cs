using Newtonsoft.Json;

namespace EMS.Desktop.Client.Models
{
    public static class Identity
    {
        private static object syncLock = new object();
        private static OAuthTokenDetails authToken;

        public static void SetIdentity(OAuthTokenDetails token)
        {
            if (token != null)
            {
                lock (syncLock)
                {
                    if (token != null)
                    {
                        authToken =
                            JsonConvert.DeserializeObject<OAuthTokenDetails>(
                                JsonConvert.SerializeObject(token));
                    }
                }
            }
        }

        public static OAuthTokenDetails AuthToken
        {
            get
            {
                return authToken;
            }
        }
    }
}
