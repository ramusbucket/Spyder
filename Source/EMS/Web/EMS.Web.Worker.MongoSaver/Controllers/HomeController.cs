using EMS.Infrastructure.DependencyInjection;
using EMS.Web.Worker.MongoSaver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Worker.MongoSaver.Controllers
{
    public class HomeController : Controller
    {
        private static CancellationToken token = new CancellationToken();
        private static List<IMongoSaver> savers;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult RunNetworkPacketsSaver()
        {
            var injector = UnityInjector.Instance;

            var type = typeof(IMongoSaver);
            var listenerTypes = Assembly.Load("EMS.Web.Worker.MongoSaver")
                .GetTypes()
                .Where(
                    x =>
                        !x.IsAbstract &&
                        !x.IsInterface &&
                        x.IsClass &&
                        type.IsAssignableFrom(x));

            savers = listenerTypes.Select(x => injector.Resolve<IMongoSaver>()).ToList();
            foreach (var saver in savers)
            {
                saver.Execute();
            }

            return this.View();
        }
    }
}
