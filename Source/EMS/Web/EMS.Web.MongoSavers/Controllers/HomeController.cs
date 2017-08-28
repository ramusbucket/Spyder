using EMS.Infrastructure.DependencyInjection;
using EMS.Web.MongoSavers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EMS.Web.MongoSavers.Models.Savers;

namespace EMS.Web.MongoSavers.Controllers
{
    public class HomeController : Controller
    {
        private static IEnumerable<IMongoSaver> _savers;

        public static IEnumerable<IMongoSaver> Savers
        {
            get
            {
                return _savers;
            }
            set
            {
                if (_savers == null)
                {
                    _savers = value;
                }
            }
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var pivotType = typeof(IMongoSaver);
            var stats = GetStatistics(Savers) ?? Enumerable.Empty<ServiceStatisticsModel>();

            return View(stats);
        }

        private IEnumerable<ServiceStatisticsModel> GetStatistics(IEnumerable<IMongoSaver> savers)
        {
            return savers?
                .Where(x => x.Statistics != null)
                .Select(x => new ServiceStatisticsModel()
                {
                    SaverName = x.GetType().Name,
                    SaverStats = x.Statistics
                });
        }
    }
}
