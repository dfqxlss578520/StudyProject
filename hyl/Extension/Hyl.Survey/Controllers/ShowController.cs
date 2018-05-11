using System;
using Hyl.Service.Survey;
using System.Web.Mvc;
using Hyl.Core.Domain.Survey;
using Hyl.Core.Domain.Survey.ViewModel;
using Hyl.Core.Helpers.Utility;
using Hyl.Survey.Infrastructure;
using StackExchange.Profiling;

namespace Hyl.Survey.Controllers
{
    [HylCustomerAuthorizeFilter]
    public class ShowController : BaseController
    {
        private readonly ISurveyDirectoryServices _surveyDirectoryServices;
        private readonly ISurveyAnswerServices _surveyAnswerServices;
        public ShowController(ISurveyDirectoryServices surveyDirectoryServices,
            ISurveyAnswerServices surveyAnswerServices)
        {
            _surveyDirectoryServices = surveyDirectoryServices;
            _surveyAnswerServices = surveyAnswerServices;
        }

        public ActionResult Index(int id)
        {
            //if (Utils.IsMobile())
            //{
            //    return RedirectToAction("Mobile", "Show", new { id = id });
            //}
            using (MiniProfiler.Current.Step("获取model"))
            {
                var model = _surveyDirectoryServices.GetDirectoryDetailStyle(id);
                if (model.SurveyState == 2 || model.SurveyDetail.EndTime < DateTime.Now || model.AnswerNum > model.SurveyDetail.EndNum)
                {
                    return RedirectToAction("Result", new ResultViewModel()
                    {
                        AnswerId = 0,
                        SurveyName = model.SurveyName,
                        ErrorTip = "该问卷已停止收集"
                    });
                }
                if (model.SurveyDetail.Rule > 1)
                {
                    return RedirectToAction("Result", new ResultViewModel()
                    {
                        AnswerId = 0,
                        SurveyName = model.SurveyName,
                        ErrorTip = "该问卷未公开"
                    });
                }
                if (model.SurveyDetail.EffectiveIp == 1)
                {
                    return RedirectToAction("Result", new ResultViewModel()
                    {
                        AnswerId = 0,
                        SurveyName = model.SurveyName,
                        ErrorTip = "每个IP只能答一次"
                    });
                }

                return View(model);
            }
        }

        public ActionResult Mobile(int id)
        {
            //if (!Utils.IsMobile())
            //{
            //    return RedirectToAction("Index", "Show", new { id = id });
            //}
            using (MiniProfiler.Current.Step("获取model"))
            {
                return View(_surveyDirectoryServices.GetDirectoryWithQuestion(id));
            }
        }

        [HttpPost]
        public ActionResult Mobile(SurveyAnswer model)
        {
            var surveyDirectory = _surveyDirectoryServices.GetDirectoryDetailStyle(model.DirId);
            if (surveyDirectory != null)
            {
                model.Uid = WebWorkContext.AdminUser == null ? 0 : WebWorkContext.AdminUser.Uid;
                model.IpAddr = Utils.GetRealIP();
                model.QuNum = surveyDirectory.SurveyQuNum;
                model.IsComplete = 1;
                model.IsEffective = 1;

                //if (surveyDirectory.SurveyDetail.Effective > 1)
                //{
                //    return Json("已经答过");
                //}
                return Json(_surveyAnswerServices.SaveAnswer(model));
            }
            return Json("找不到问卷信息");
        }

        public ActionResult Result(ResultViewModel model)
        {
            if (string.IsNullOrEmpty(model.ErrorTip))
            {
                model.ErrorTip = "问卷提交失败！";
            }
            return View(model);
        }

    }
}