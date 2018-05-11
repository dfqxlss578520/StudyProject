using StackExchange.Profiling;
using System.Web.Mvc;
using Hyl.Core.Job;
using Hyl.Core.Logs;
using Hyl.Service.JobManager;
using Hyl.WebMvc.Models;
using NHibernate;
using Hyl.Repository;

namespace Hyl.WebMvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogs<HomeController> _log;
        private readonly IJobManager _jobManager;
        private readonly IRepository<WindDirection> _reposity;
        public HomeController(ILogs<HomeController> logs, IJobManager jobManager, IRepository<WindDirection> reposity)
        {
            _log = logs;
            _jobManager = jobManager;
            _reposity = reposity;
        }

        public ActionResult Index()
        {
            //WindDirection model = new WindDirection()
            //{
            //    Code = "10",
            //    CnName = "上风",
            //    EnName = "UpWind"
            //};
            //var rst = _reposity.Add(model);
            //NHibernateHelper.getSession();
            //ISession session = NHibernateHelper.getSession();
            //ITransaction transaction = session.BeginTransaction();
            //var rst = session.QueryOver<WindDirection>().List();
            //transaction.Commit();
            //session.Close();

            using (MiniProfiler.Current.Step("查询ID"))
            {
                var log = _log;
                if (log.IsErrorEnabled)
                {
                    log.Error("错误内容");
                }
            }
            //    _jobManager.AddJob<TestJob>("testjob", "testgroup", "/10 * * ? * *");
                //    _jobManager.StartAllJob();

                //}
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