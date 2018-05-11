using System.Web.Mvc;
using Hyl.Core.Job;
using Hyl.Core.Logs;
using StackExchange.Profiling;

namespace Hyl.Survey.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogs<HomeController> _log;
        private readonly IJobManager _jobManager;
        public HomeController(ILogs<HomeController> logs, IJobManager jobManager)
        {
            _log = logs;
            _jobManager = jobManager;
        }
        public ActionResult Index()
        {
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