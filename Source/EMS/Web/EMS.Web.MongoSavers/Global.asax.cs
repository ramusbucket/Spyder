using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EMS.Web.MongoSavers.App_Start;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EMS.Infrastructure.DependencyInjection;
using EMS.Web.MongoSavers.Controllers;
using EMS.Web.MongoSavers.Models.Savers;

namespace EMS.Web.MongoSavers
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            new DependencyInjectionConfig().RegisterDependencies();

            HomeController.Savers = StartSavers().Result;
        }

        private Task<List<IMongoSaver>> StartSavers()
        {
            var injector = UnityInjector.Instance;

            var type = typeof(IMongoSaver);
            var saversTypes = Assembly.Load("EMS.Web.MongoSavers")
                .GetTypes()
                .Where(
                    x =>
                        !x.IsAbstract &&
                        !x.IsInterface &&
                        x.IsClass &&
                        type.IsAssignableFrom(x));

            var savers = saversTypes.Select(x => injector.Resolve<IMongoSaver>(x.Name)).ToList();
            foreach (var saver in savers)
            {
                Task.Run(() => saver.Start());
            }

            return Task.FromResult(savers);
        }
    }
}
