using System.Web.Mvc;
using System.Web.Routing;
using Hyl.Core.Domain.Survey;
using Hyl.Core.Domain.Survey.ViewModel;
using Hyl.Core.Helpers.Utility;
using Hyl.Repository;
using Hyl.Service.Survey;
using Hyl.Survey.Infrastructure;
using StackExchange.Profiling;

namespace Hyl.Survey.Areas.Account.Controllers
{
    [RoutePrefix("Survey")]
    public class SurveyController : AccountBaseController
    {
        private readonly ISurveyDirectoryServices _surveyDirectoryServices;
        private readonly ISurveyStyleServices _surveyStyleServices;
        public SurveyController(ISurveyDirectoryServices surveyDirectoryServices,
            ISurveyStyleServices surveyStyleServices)
        {
            _surveyDirectoryServices = surveyDirectoryServices;
            _surveyStyleServices = surveyStyleServices;
        }


        #region 页面主方法
        /// <summary>
        /// 问卷列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Index(SurveyDirectoryViewModel model)
        {
            ViewBag.seachModel = model;
            model.QuerySurveyState = model.QuerySurveyState ?? string.Empty;
            model.QuerySurveyName = model.QuerySurveyName ?? string.Empty;
            model.Uid = WebWorkContext.AdminUser.Uid;
            var pagedData = _surveyDirectoryServices.GetPaged(model).ToMvcPager();
            return View(pagedData);
        }

        /// <summary>
        /// 创建问卷
        /// </summary>
        /// <param name="surveyName"></param>
        /// <returns></returns>
        public ActionResult Create(string surveyName)
        {
            SurveyDirectory surveyDirectory = new SurveyDirectory
            {
                SurveyName = Utils.UrlDecode(surveyName),
                DirType = 2,
                Uid = WebWorkContext.AdminUser.Uid,
                Sid = Utils.GetCheckCode(8)
            };
            long surveyId = _surveyDirectoryServices.Insert(surveyDirectory);
            return RedirectToAction("Index", "SurveyDesign", new RouteValueDictionary()
            {
                {"surveyId",surveyId }
            });
        }

        /// <summary>
        /// 最终发布页，分享发布地址等
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult Share(int surveyId, string act = "")
        {
            ViewBag.act = act;
            var userServeyModel = _surveyDirectoryServices.GetDirectoryDetailStyle(surveyId, WebWorkContext.AdminUser.Uid);
            if (userServeyModel != null && userServeyModel.Id > 0)
            {
                userServeyModel.SurveyName = Utils.UrlDecode(userServeyModel.SurveyName);
                return View(userServeyModel);
            }
            return RedirectToAction("Index", "Survey");
        }

        /// <summary>
        /// 复制问卷
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult CopySurvey(int surveyId)
        {
            _surveyDirectoryServices.Copy(surveyId);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult Delete(int surveyId)
        {
            var userServeyModel = _surveyDirectoryServices.GetDirectoryDetailStyle(surveyId, WebWorkContext.AdminUser.Uid);
            if (userServeyModel != null && userServeyModel.Id > 0)
            {
                _surveyDirectoryServices.Delete(userServeyModel.Id);
            }
            return RedirectToAction("Index", "Survey");
        }
        #endregion




        #region 辅助Action

        /// <summary>
        /// 设计界面 - 更新问卷设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Update(SurveyDirectory model)
        {
            var userServeyModel = _surveyDirectoryServices.GetDirectoryWithQuestion(model.Id, WebWorkContext.AdminUser.Uid);
            if (userServeyModel != null && userServeyModel.Id > 0)
            {
                userServeyModel.SurveyName = model.SurveyName;
                userServeyModel.SurveyDetail = model.SurveyDetail;
                var detail = _surveyDirectoryServices.UpdateSurveyNameAndDetail(userServeyModel);
                return Json(detail.ToString().ToLower());
            }
            return Json("false");
        }

        /// <summary>
        /// 设计界面 - 发布，可设定收集规则、样式模板
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult PrePublish(int surveyId, string editstyle = "")
        {
            using (MiniProfiler.Current.Step("Question Miniprofiler"))
            {
                ViewBag.action = editstyle;
                var userServeyModel = _surveyDirectoryServices.GetDirectoryDetailStyle(surveyId, WebWorkContext.AdminUser.Uid);
                if (userServeyModel != null && userServeyModel.Id > 0)
                {
                    userServeyModel.SurveyName = Utils.UrlDecode(userServeyModel.SurveyName);
                    //userServeyModel.Questions = null;

                    return View(userServeyModel);
                }
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 样式页面 - 更新问卷状态为 发布
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult Publish(int surveyId)
        {
            using (MiniProfiler.Current.Step("Question Miniprofiler"))
            {
                if (_surveyDirectoryServices.Publish(surveyId, WebWorkContext.AdminUser.Uid))
                {
                    return RedirectToAction("Share", new { surveyId = surveyId });
                }
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 样式页面 - 下载样式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("style/get/{id?}", Name = "styleget")]
        public ActionResult StyleGet(int id)
        {
            return Json(_surveyStyleServices.Get(id));
        }

        /// <summary>
        /// 样式页面 - 保存样式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StyleSave(SurveyDirectory model)
        {
            var userServeyModel = _surveyDirectoryServices.GetDirectoryWithQuestion(model.Id, WebWorkContext.AdminUser.Uid);
            if (userServeyModel != null && userServeyModel.Id > 0)
            {
                model.SurveyDetail.Id = userServeyModel.SurveyDetailId;
                model.SurveyDetail.DirId = userServeyModel.Id;
                var detail = _surveyDirectoryServices.UpdateSurveyStyle(model);

                return Json(detail.ToString().ToLower());
            }
            return Json("false");
        }

        /// <summary>
        /// 收集答卷 - 更改问卷状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="surveyState"></param>
        /// <returns></returns>
        public ActionResult SurveyState(int id, int surveyState)
        {
            return Json(_surveyDirectoryServices.SaveSurveyState(id, WebWorkContext.AdminUser.Uid, surveyState));
        }

        #endregion


    }
}