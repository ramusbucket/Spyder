using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Server.Collector
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
