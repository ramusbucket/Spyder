using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace EMS.Desktop.Client.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(HttpResponseMessage response)
        {
            this.Response = response;
        }

        public HttpResponseMessage Response { get; set; }

        public HttpStatusCode StatusCode
        {
            get
            {
                return this.Response.StatusCode;
            }
        }

        public IEnumerable<string> Errors
        {
            get
            {
                return this.Data.Values.Cast<string>().ToList();
            }
        }

        public static ApiException FromHttpResponseMessage(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            var anonymousErrorObject =
                new { message = "", ModelState = new Dictionary<string, string[]>() };

            var deserializedErrorObject =
                JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            var ex = new ApiException(response);

            if (deserializedErrorObject.ModelState != null)
            {
                var errors = deserializedErrorObject.ModelState
                    .Select(kvp => string.Join(". ", kvp.Value));
                for (int i = 0; i < errors.Count(); i++)
                {
                    ex.Data.Add(i, errors.ElementAt(i));
                }
            }

            else
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
                foreach (var kvp in error)
                {
                    ex.Data.Add(kvp.Key, kvp.Value);
                }
            }
            return ex;
        }
    }
}
