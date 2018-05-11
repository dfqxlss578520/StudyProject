using System.Web.Mvc;
using Hyl.Core.Job;
using Hyl.Core.Logs;
using Hyl.Service.JobManager;
using Hyl.Service.MemberCenters;

namespace Hyl.SSO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogs<HomeController> _log;
        private readonly IUsersServices _usersServices;
        public HomeController(ILogs<HomeController> logs, IUsersServices usersServices)
        {
            _log = logs;
            _usersServices = usersServices;
        }

        public ActionResult Index()
        {
            var model = _usersServices.GetById(1);
            if (model != null)
            {

            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}