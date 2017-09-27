using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EMS.Core.Models;
using EMS.Core.Models.Mongo;
using EMS.Web.Website.Models;
using EMS.Web.Website.Services;

namespace EMS.Web.Website.Controllers
{
    public class MonitoringSessionsController : Controller
    {
        private readonly IMonitoringSessionsService _monitoringSessionsService;

        public MonitoringSessionsController()
        {
            _monitoringSessionsService = new MonitoringSessionsService();
        }

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int itemsPerPage = 12)
        {
            var model = new SessionsWithDetailsViewModel();

            try
            {
                model.Sessions = await _monitoringSessionsService.GetActiveSessionsDetails(page, itemsPerPage);
            }
            catch (Exception exc)
            {
                model.Sessions = Enumerable.Empty<SessionViewModel>();
                model.Message = exc.Message;
            }

            return View(model);
        }
    }
}