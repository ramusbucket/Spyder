using System.Collections.Generic;

namespace EMS.Web.Website.Models
{
    public class SessionsWithDetailsViewModel
    {
        public string Message { get; set; }

        public IEnumerable<SessionViewModel> Sessions { get; set; }
    }
}