using System.Configuration;
using System.Web.Mvc;
using Hyl.SSO.ClientHelper;
using Hyl.Survey.Infrastructure;
using Newtonsoft.Json;

namespace Hyl.Survey.Areas.Account.Controllers
{
    public class AuthController : Controller
    {
        // GET: Account/Login
        public ActionResult Login(string rurl)
        {
            //if (ConfigurationSettings.AppSettings["IsUseSSO"] != null && ConfigurationSettings.AppSettings["IsUseSSO"].ToLower() == "true")
            //{
            SsoModel ssoModel = new SsoModel();
            ssoModel.Appkey = ConfigurationManager.AppSettings["SsoAppId"];
            ssoModel.Appsecret = ConfigurationManager.AppSettings["SsoAppSecret"];
            ssoModel.SsoBaseUrl = ConfigurationManager.AppSettings["SsoHost"];

            if (ssoModel.IsNeedValidateHylId())
            {
                string userinfoStr = ssoModel.ValidateHylId();
                ResponseModel<Users> userModel = JsonConvert.DeserializeObject<ResponseModel<Users>>(userinfoStr);
                if (userModel != null && userModel.Data != null)
                {
                    WebWorkContext.AdminUser = userModel.Data;
                    WebWorkContext.AuthorityId = (int)AuthorityEnum.Admin;
                }
            }

            if (WebWorkContext.AdminUser == null)
            {
                ssoModel.ToSsoPage(Request.Url.AbsoluteUri);
            }
            if (!string.IsNullOrEmpty(rurl))
            {
                return Redirect(rurl);
            }
            return RedirectToAction("Index", "Survey");
            //}
        }

        public ActionResult LoginOut()
        {
            WebWorkContext.LoginOut();
            //if (ConfigurationSettings.AppSettings["IsUseSSO"] != null &&
            //    ConfigurationSettings.AppSettings["IsUseSSO"].ToLower() == "true")
            //{
            SsoModel ssoModel = new SsoModel();
            ssoModel.Appkey = ConfigurationManager.AppSettings["SsoAppId"];
            ssoModel.Appsecret = ConfigurationManager.AppSettings["SsoAppSecret"];
            ssoModel.SsoBaseUrl = ConfigurationManager.AppSettings["SsoHost"];
            ssoModel.LoginOut(Request.Url.AbsoluteUri.ToLower().Replace("loginout", "Login"));
            //}
            //else
            //{
            //    return RedirectToAction("Index");
            //}
            return RedirectToAction("Login");
        }

        [HylSurveyMvcAuthorizeFilter]
        public ActionResult NoAuthorize(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.msg = msg;
            }
            else
            {
                ViewBag.msg = "您没有权限查看";
            }
            return View();
        }

        public ActionResult Portal(string u, string rurl = "")
        {
            if (WebWorkContext.AdminUser == null)
            {
                ResponseModel<Users> userModel = JsonConvert.DeserializeObject<ResponseModel<Users>>(u);
                if (userModel != null && userModel.Data != null)
                {
                    WebWorkContext.AdminUser = userModel.Data;
                    if (!string.IsNullOrEmpty(rurl))
                    {
                        return Redirect(rurl);
                    }
                    return RedirectToAction("Index", "Survey");
                }
            }
            return View();

        }

    }
}