using System;
using Newtonsoft.Json;

namespace EMS.Desktop.Client.Models
{
    public static class Identity
    {
        private static readonly object SyncLock = new object();
        private static OAuthTokenDetails _authToken;
        private static string _sessionId;

        public static void SetIdentity(OAuthTokenDetails token)
        {
            if (token != null)
            {
                lock (SyncLock)
                {
                    if (token != null)
                    {
                        _authToken =
                            JsonConvert.DeserializeObject<OAuthTokenDetails>(
                                JsonConvert.SerializeObject(token));

                        _sessionId = Guid.NewGuid().ToString();
                    }
                }
            }
        }

        public static OAuthTokenDetails AuthToken => _authToken;

        public static string SessionId => _sessionId;
    }
}
