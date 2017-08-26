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
        public static IEnumerable<IMongoSaver> savers;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var pivotType = typeof(IMongoSaver);
            var savers = Assembly.GetAssembly(pivotType)
                .GetTypes()
                .Where(x => pivotType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface && x.IsClass)
                .Select(x => x.Name)
                .ToList();

            return View(savers);
        }

        public async Task<JsonResult> StartSavers()
        {
            var injector = UnityInjector.Instance;

            var type = typeof(IMongoSaver);
            var saversTypes = Assembly.Load("EMS.Web.Worker.MongoSaver")
                .GetTypes()
                .Where(
                    x =>
                        !x.IsAbstract &&
                        !x.IsInterface &&
                        x.IsClass &&
                        type.IsAssignableFrom(x));

            savers = saversTypes.Select(x => injector.Resolve<IMongoSaver>(x.Name)).ToList();
            foreach (var saver in savers)
            {
                Task.Run(() => saver.Start());
            }

            return this.Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
