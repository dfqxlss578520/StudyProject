using System.Web.Mvc;
using Hyl.Service.Survey;
using Hyl.Survey.Infrastructure;
using StackExchange.Profiling;
using Hyl.Core.Domain.Survey;
using Hyl.Core.Domain.PageDomain;

namespace Hyl.Survey.Areas.Account.Controllers
{
    public class ReportController : AccountBaseController
    {
        private readonly ISurveyDirectoryServices _surveyDirectoryServices;
        private readonly ISurveyAnswerServices _surveyAnswerServices;
        public ReportController(ISurveyDirectoryServices surveyDirectoryServices,
            ISurveyAnswerServices surveyAnswerServices)
        {
            _surveyDirectoryServices = surveyDirectoryServices;
            _surveyAnswerServices = surveyAnswerServices;
        }

        /// <summary>
        /// 默认报告
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult Index(int surveyId)
        {
            using (MiniProfiler.Current.Step("Question Miniprofiler"))
            {
                var userServeyModel = _surveyDirectoryServices.GetDirectoryDetailStyle(surveyId, WebWorkContext.AdminUser.Uid);
                if (userServeyModel != null && userServeyModel.Id > 0)
                {
                    userServeyModel.Questions = _surveyAnswerServices.GetStatistics(userServeyModel);
                    return View(userServeyModel);
                }
                return RedirectToAction("NoAuthorize", "Auth");
            }
        }

        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Original(int surveyId, int id = 1)
        {
            using (MiniProfiler.Current.Step("Question Miniprofiler"))
            {
                var userServeyModel = _surveyDirectoryServices.GetDirectoryDetailStyle(surveyId, WebWorkContext.AdminUser.Uid);
                if (userServeyModel != null && userServeyModel.Id > 0)
                {
                    userServeyModel.PageSurveyAnswer = _surveyAnswerServices.GetPageSurveyAnswer(new Page<SurveyAnswer>()
                    {
                        PageIndex = id,
                        PageSize = 10,
                        OrderBy = "Id desc",
                        Conditions = "Where IsValid =1 AND DirId="+surveyId
                    });
                    return View(userServeyModel);
                }
                return RedirectToAction("NoAuthorize", "Auth");
            }
        }

        /// <summary>
        /// 图标数据
        /// </summary>
        /// <param name="quId"></param>
        /// <returns></returns>
        public ActionResult ChartData(int quId)
        {
            return Json(_surveyAnswerServices.GetChart(quId));
        }

        /// <summary>
        /// 查看问卷答案
        /// </summary>
        /// <param name="answerId"></param>
        /// <returns></returns>
        public ActionResult ViewAnswer(int answerId)
        {
            if (answerId > 0)
            {
                return View(_surveyAnswerServices.GetAnswers(answerId));
            }
            return Content("找不到答卷信息");
        }

        /// <summary>
        /// 删除答案
        /// </summary>
        /// <param name="answerId"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult DeleteAnswer(int answerId,int surveyId)
        {
            _surveyAnswerServices.Delete(answerId);
            return RedirectToAction("Original",new { surveyId = surveyId });
        }
    }
}