using System.Web.Mvc;
using Hyl.Core.Domain.Survey;
using Hyl.Core.Helpers.Utility;
using Hyl.Service.Survey;
using Hyl.Survey.Infrastructure;
using StackExchange.Profiling;

namespace Hyl.Survey.Areas.Account.Controllers
{
    public class SurveyDesignController : AccountBaseController
    {
        private readonly ISurveyDirectoryServices _surveyDirectoryServices;
        private readonly ISurveyDetailServices _surveyDetailServices;
        private readonly IQuestionServices _questionServices;
        public SurveyDesignController(ISurveyDirectoryServices surveyDirectoryServices,
            IQuestionServices questionServices,
            ISurveyDetailServices surveyDetailServices)
        {
            _surveyDirectoryServices = surveyDirectoryServices;
            _surveyDetailServices = surveyDetailServices;
            _questionServices = questionServices;
        }

        /// <summary>
        /// 显示问卷内容
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
                    userServeyModel.SurveyName = Utils.UrlDecode(userServeyModel.SurveyName);

                    return View(userServeyModel);
                }
                return RedirectToAction("NoAuthorize", "Auth");
            }
        }

        /// <summary>
        /// 保存试题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Question question)
        {
            question.Uid = WebWorkContext.AdminUser.Uid;
            var rst = _questionServices.Save(question);
            return Json(rst);
        }

        #region Delete        
        public ActionResult DeleteQuestion(int id)
        {
            var rst = _questionServices.DeleteQuestion(WebWorkContext.AdminUser.Uid, id) > 0;
            return Content(rst.ToString().ToLower());
        }
        public ActionResult DeleteRadio(int id)
        {
            var rst = _questionServices.DeleteQuestionRadio(WebWorkContext.AdminUser.Uid, id) > 0;
            return Content(rst.ToString().ToLower());
        }
        public ActionResult DeleteCheckbox(int id)
        {
            var rst = _questionServices.DeleteQuestionCheckbox(WebWorkContext.AdminUser.Uid, id) > 0;
            return Content(rst.ToString().ToLower());
        }
        public ActionResult DeleteScore(int id)
        {
            var rst = _questionServices.DeleteQuestionScore(WebWorkContext.AdminUser.Uid, id) > 0;
            return Content(rst.ToString().ToLower());
        }
        public ActionResult DeleteOrderby(int id)
        {
            var rst = _questionServices.DeleteQuestionOrderby(WebWorkContext.AdminUser.Uid, id) > 0;
            return Content(rst.ToString().ToLower());
        }
        public ActionResult DeleteMultiFillblank(int id)
        {
            var rst = _questionServices.DeleteQuestionMultiFillblank(WebWorkContext.AdminUser.Uid, id) > 0;
            return Content(rst.ToString().ToLower());
        }
        public ActionResult DeleteChenColumn(int id)
        {
            var rst = _questionServices.DeleteQuestionChenColumn(WebWorkContext.AdminUser.Uid, id) > 0;
            return Content(rst.ToString().ToLower());
        }
        public ActionResult DeleteChenRow(int id)
        {
            var rst = _questionServices.DeleteQuestionChenRow(WebWorkContext.AdminUser.Uid, id) > 0;
            return Content(rst.ToString().ToLower());
        }
        #endregion

    }
}