using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Hyl.Core.Caching;
using Hyl.Core.Configuration;
using Hyl.Core.Domain.MemberCenters;
using Hyl.Core.Helpers.Utility;
using Hyl.Core.Logs;
using Hyl.Service.MemberCenters;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Newtonsoft.Json;

namespace Hyl.SSO.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly ILogs<AuthorizeController> _log;
        private readonly IUsersServices _usersServices;
        private readonly IUsersSsoTokensServices _ssoTokensServices;
        private readonly IAppInfosServics _appInfosServics;
        private readonly IUserSnsRelationServics _userSnsRelationServics;
        public AuthorizeController(ILogs<AuthorizeController> logs,
            IUsersServices usersServices,
            IUsersSsoTokensServices ssoTokensServices,
            IAppInfosServics appInfosServics,
            ICacheManager cacheManager,
            IUserSnsRelationServics userSnsRelationServics)
        {
            _log = logs;
            _usersServices = usersServices;
            _ssoTokensServices = ssoTokensServices;
            _appInfosServics = appInfosServics;
            _userSnsRelationServics = userSnsRelationServics;
        }

        #region 登录
        /// <summary>
        /// 登录页面初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.returnurl = Request.QueryString["returnurl"];
            var companyid = Request.QueryString["CompanyId"];

            var cookie = Request.Cookies[BaseConfig.HylSsoToken];
            if (cookie != null)
            {
                //自动登录
                var tokenModel = _ssoTokensServices.GetByToken(cookie.Value);
                if (tokenModel != null)
                {
                    //合法cookie
                    if (!string.IsNullOrEmpty(companyid)) 
                    {
                        //附带公司参数
                        var user = _usersServices.GetById(tokenModel.Uid);
                        if (user != null && user.Company == Utils.StrToInt(companyid))
                        {
                            //依据公司参数，身份合法，返回原地址
                            cookie.Expires = DateTime.Now.AddYears(1);
                            Response.AppendCookie(cookie);
                            if (!string.IsNullOrEmpty(Request["returnurl"]))
                            {
                                return Redirect(Utils.UrlAppendParameter(Request["returnurl"], string.Format("{0}={1}", BaseConfig.HylSsoReturnParameter, cookie.Value)));
                            }
                        }
                        else
                        {
                            //身份不合法，不是当前公司参数的cookie，应当清除
                            cookie.Expires = DateTime.Now.AddYears(-10);
                            Response.AppendCookie(cookie);
                        }
                    }
                    else
                    {
                        //未附带公司参数，默认处理
                        cookie.Expires = DateTime.Now.AddYears(1);
                        Response.AppendCookie(cookie);

                        if (!string.IsNullOrEmpty(Request["returnurl"]))
                        {
                            return Redirect(Utils.UrlAppendParameter(Request["returnurl"], string.Format("{0}={1}", BaseConfig.HylSsoReturnParameter, cookie.Value)));
                        }
                    }
                }
                
            }
            ViewBag.companyid = companyid;
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Users model)
        {
            var rurl = Request["returnurl"];
            var cpyid = Request["CompanyId"];
            if (ModelState.IsValid)
            {
                Users rst = new Users();
                if (!string.IsNullOrEmpty(cpyid) && Utils.StrToInt(cpyid) > 0)
                {
                    rst = _usersServices.GetByUsernameCompany(model.UserName, Utils.StrToInt(cpyid));
                }
                else
                {
                    rst = _usersServices.GetByUsername(model.UserName);
                }
                if (rst != null && rst.Password == Encrypt.StrongEncrypt(rst.Salt, model.Password))
                {
                    var tokenModel = _ssoTokensServices.GetUserToken(rst);
                    HttpCookie cookie = new HttpCookie(BaseConfig.HylSsoToken);
                    cookie.Value = tokenModel.Token;
                    cookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(cookie);

                    return Redirect(Utils.UrlAppendParameter(Request["returnurl"], string.Format("{0}={1}", BaseConfig.HylSsoReturnParameter, tokenModel.Token)));
                }
            }
            ViewBag.returnurl = rurl;
            ViewBag.companyid = cpyid;
            return View(model);
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="token"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult AutoLogin(string appkey, string token, string username, string password, string returnUrl, string CompanyId = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(token))
                {
                    //验证token信息
                    var appinfo = _appInfosServics.GetByAppId(appkey);
                    var veryifyToken = Encrypt.EncryptMD5(appinfo.AppKey + appinfo.AppSecret + BaseConfig.HylSsoTokenEncryptSalt);
                    if (veryifyToken == token && !string.IsNullOrEmpty(username))
                    {
                        //验证用户信息
                        Users rst = new Users();
                        if (!string.IsNullOrEmpty(CompanyId) && Utils.StrToInt(CompanyId) > 0)
                        {
                            rst = _usersServices.GetByUsernameCompany(username, Utils.StrToInt(CompanyId));
                        }
                        else
                        {
                            rst = _usersServices.GetByUsername(username);
                        }
                        if (rst != null && rst.Password == Encrypt.StrongEncrypt(rst.Salt, password))
                        {
                            //写入身份token Cookie
                            var tokenModel = _ssoTokensServices.GetUserToken(rst);
                            HttpCookie cookie = new HttpCookie(BaseConfig.HylSsoToken);
                            cookie.Value = tokenModel.Token;
                            cookie.Expires = DateTime.Now.AddYears(1);
                            Response.Cookies.Add(cookie);
                            if (returnUrl.ToLower().Contains("usr={}"))
                            {
                                returnUrl = returnUrl.Replace("usr={}", "");
                            }
                            return Redirect(returnUrl);
                        }
                        return Redirect(returnUrl);
                    }
                }
                return Redirect(returnUrl);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return Redirect(returnUrl);
            }
        }
        #endregion


        #region 验证登录
        /// <summary>
        /// SSO-client验证hylid
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="token"></param>
        /// <param name="hylid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Validate(string appkey, string token, string hylid)
        {
            try
            {
                if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(token))
                {
                    var appinfo = _appInfosServics.GetByAppId(appkey);
                    var veryifyToken = Encrypt.EncryptMD5(appinfo.AppKey + appinfo.AppSecret + BaseConfig.HylSsoTokenEncryptSalt);
                    if (veryifyToken == token)
                    {
                        if (!string.IsNullOrEmpty(hylid))
                        {
                            var tokenModel = _ssoTokensServices.GetByToken(hylid);
                            if (tokenModel != null)
                            {
                                var user = _usersServices.GetById(tokenModel.Uid);
                                var userViewModel = Mapper.Map<Users, UserValidateViewModel>(user);

                                var model = new UserSsoValidation<UserValidateViewModel> { Rst = true, Data = userViewModel };
                                return Json(model);
                            }
                        }
                        return Json(new UserSsoValidation<Users> { Rst = false, Msg = "hylid信息缺失，请检查", Data = null });
                    }
                }
                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "应用信息错误，请检查AppId信息", Data = null });
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "应用执行错误", Data = null });
            }
        }

        /// <summary>
        /// 获取当前SSO-client的登录用户信息
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="token"></param>
        /// <param name="returnurl"></param>
        /// <param name="openid">用Openid登录（可省略）</param>
        /// <param name="unionid">用unionid登录（可省略）</param>
        /// <returns></returns>
        public ActionResult TryGetUserInfo(string appkey, string token, string returnurl, string openid, string unionid)
        {
            try
            {
                if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(token))
                {
                    var appinfo = _appInfosServics.GetByAppId(appkey);
                    var veryifyToken = Encrypt.EncryptMD5(appinfo.AppKey + appinfo.AppSecret + BaseConfig.HylSsoTokenEncryptSalt);
                    if (veryifyToken == token)
                    {
                        #region Cookie登录
                        if (Request.Cookies[BaseConfig.HylSsoToken] != null)
                        {
                            var cookie = Request.Cookies[BaseConfig.HylSsoToken];
                            var tokenModel = _ssoTokensServices.GetByToken(cookie.Value);
                            if (tokenModel != null)
                            {
                                cookie.Expires = DateTime.Now.AddYears(1);
                                Response.AppendCookie(cookie);

                                var jsonobj = JsonConvert.SerializeObject(new UserSsoValidation<Users> { Rst = true, Msg = "", Data = _usersServices.GetById(tokenModel.Uid) });
                                var url = Utils.UrlAppendParameter(returnurl, string.Format("{0}={1}", "usr", Utils.UrlEncode(Encrypt.EncrypRijndael(jsonobj))));
                                return Redirect(url);
                            }
                        }
                        #endregion

                        #region Openid或UnionId登录
                        if (!string.IsNullOrEmpty(openid) || !string.IsNullOrEmpty(unionid))
                        {
                            var useOpenid = !string.IsNullOrEmpty(openid) ? openid : unionid;
                            var snsRelation = _userSnsRelationServics.Get(useOpenid);
                            if (snsRelation?.User != null)
                            {
                                //write cookie(if it works)
                                Response.AppendCookie(new HttpCookie(BaseConfig.HylSsoToken) { Expires = DateTime.Now.AddYears(1) });

                                var jsonobj = JsonConvert.SerializeObject(new UserSsoValidation<Users> { Rst = true, Msg = "", Data = snsRelation.User });
                                var url = Utils.UrlAppendParameter(returnurl, string.Format("{0}={1}", "usr", Utils.UrlEncode(Encrypt.EncrypRijndael(jsonobj))));
                                return Redirect(url);
                            }
                        }
                        #endregion

                        return Redirect(Utils.UrlAppendParameter(returnurl, string.Format("{0}={1}", "usr", "{}")));
                    }
                }
                return Redirect(Utils.UrlAppendParameter(returnurl, string.Format("{0}={1}", "usr", "{}")));
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return Redirect(Utils.UrlAppendParameter(returnurl, string.Format("{0}={1}", "usr", "{}")));
            }
        }

        /// <summary>
        /// 绑定SNS账户
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="token"></param>
        /// <param name="openid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult TryBindSnsRelation(string appkey, string token, string openid, long uid)
        {
            try
            {
                if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(token))
                {
                    var appinfo = _appInfosServics.GetByAppId(appkey);
                    var veryifyToken = Encrypt.EncryptMD5(appinfo.AppKey + appinfo.AppSecret + BaseConfig.HylSsoTokenEncryptSalt);
                    if (veryifyToken == token)
                    {
                        var user = _usersServices.GetById(uid);
                        if (user != null && user.Uid > 0)
                        {
                            var relation = _userSnsRelationServics.Get(openid, uid);
                            if (relation == null)
                            {
                                var relationId = _userSnsRelationServics.Insert(new UserSnsRelation() { OpenId = openid, Uid = uid });
                                if (relationId > 0)
                                {
                                    return Json(new UserSsoValidation<Users> { Rst = true, Msg = "绑定成功", Data = null });
                                }
                                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "绑定失败", Data = null });
                            }
                            return Json(new UserSsoValidation<Users> { Rst = true, Msg = "绑定成功", Data = null });
                        }
                        return Json(new UserSsoValidation<Users> { Rst = false, Msg = "用户不存在", Data = null });
                    }
                    return Json(new UserSsoValidation<Users> { Rst = false, Msg = "加密验证失败", Data = null });
                }
                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "appkey或token不存在", Data = null });
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "执行出现错误", Data = null });
            }
        }
        #endregion



        #region 登出
        /// <summary>
        /// 注销登出
        /// </summary>
        /// <param name="returnurl"></param>
        /// <returns></returns>
        public ActionResult Logout(string returnurl)
        {
            if (Request.Cookies[BaseConfig.HylSsoToken] != null && !string.IsNullOrEmpty(Request.Cookies[BaseConfig.HylSsoToken].Value))
            {
                var cookie = Request.Cookies[BaseConfig.HylSsoToken];
                if (!string.IsNullOrEmpty(cookie.Value))
                {
                    var tokenModel = _ssoTokensServices.GetByToken(cookie.Value);
                    if (tokenModel != null)
                    {
                        cookie.Expires = DateTime.Now.AddYears(-100);
                    }
                    Response.AppendCookie(cookie);
                }
            }
            return Redirect(returnurl);
        }
        #endregion



        #region Register
        /// <summary>
        /// 检测账号是否被注册
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="token"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TryRegister(string appkey, string token, string username, string password, string CompanyId = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(token))
                {
                    var appinfo = _appInfosServics.GetByAppId(appkey);
                    var veryifyToken = Encrypt.EncryptMD5(appinfo.AppKey + appinfo.AppSecret + BaseConfig.HylSsoTokenEncryptSalt);
                    if (veryifyToken == token && !string.IsNullOrEmpty(username))
                    {
                        Users rst = new Users();
                        if (!string.IsNullOrEmpty(CompanyId) && Utils.StrToInt(CompanyId) > 0)
                        {
                            rst = _usersServices.GetByUsernameCompany(username, Utils.StrToInt(CompanyId));
                        }
                        else
                        {
                            rst = _usersServices.GetByUsername(username);
                        }
                        if (rst != null)
                        {
                            return Json(new UserSsoValidation<Users> { Rst = true, Msg = "用户已存在", Data = new Users() { Uid = rst.Uid } });
                        }
                        Users user = new Users();
                        user.UserName = username;
                        user.Salt = Guid.NewGuid().ToString().Split('-')[0];
                        user.Password = Encrypt.StrongEncrypt(user.Salt, password);
                        user.IsValid = true;
                        user.Company = !string.IsNullOrEmpty(CompanyId) && Utils.StrToInt(CompanyId) > 0 ? Utils.StrToInt(CompanyId) : 0;

                        var uid = _usersServices.Insert(user);
                        return Json(new UserSsoValidation<Users> { Rst = true, Msg = "保存成功", Data = new Users() { Uid = uid } });
                    }
                }
                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "应用信息错误，请检查AppId信息", Data = null });
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "应用执行错误", Data = null });
            }
        }

        ///// <summary>
        ///// 注册
        ///// </summary>
        ///// <param name="appkey"></param>
        ///// <param name="token"></param>
        ///// <param name="username"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Register(string appkey, string token, string username, string password)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(token))
        //        {
        //            var appinfo = _appInfosServics.GetByAppId(appkey);
        //            var veryifyToken = Encrypt.EncryptMD5(appinfo.AppKey + appinfo.AppSecret + BaseConfig.HylSsoTokenEncryptSalt);
        //            if (veryifyToken == token && !string.IsNullOrEmpty(username))
        //            {
        //                Users user = new Users();
        //                user.UserName = username;
        //                user.Salt = Guid.NewGuid().ToString().Split('-')[0];
        //                user.Password = Encrypt.StrongEncrypt(user.Salt, password);
        //                user.IsValid = true;

        //                _usersServices.Insert(user);

        //                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "保存成功", Data = null });
        //            }
        //        }
        //        return Json(new UserSsoValidation<Users> { Rst = false, Msg = "应用信息错误，请检查AppId信息", Data = null });
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Error(e.Message);
        //        return Json(new UserSsoValidation<Users> { Rst = false, Msg = "应用执行错误", Data = null });
        //    }

        //}

        ///// <summary>
        ///// 批量注册
        ///// </summary>
        ///// <param name="appkey"></param>
        ///// <param name="token"></param>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult BatchRegister(string appkey, string token, List<NewUsersModel> list)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(token))
        //        {
        //            var appinfo = _appInfosServics.GetByAppId(appkey);
        //            var veryifyToken = Encrypt.EncryptMD5(appinfo.AppKey + appinfo.AppSecret + BaseConfig.HylSsoTokenEncryptSalt);
        //            if (veryifyToken == token && list != null && list.Count > 0)
        //            {
        //                List<Users> userlist = new List<Users>();
        //                foreach (var item in list)
        //                {
        //                    Users user = new Users();
        //                    user.UserName = item.Username;
        //                    user.Salt = Guid.NewGuid().ToString().Split('-')[0];
        //                    user.Password = Encrypt.StrongEncrypt(user.Salt, item.Password);
        //                    user.IsValid = true;
        //                    userlist.Add(user);
        //                }
        //                _usersServices.InsertAddBatch(userlist);

        //                return Json(new UserSsoValidation<Users> { Rst = false, Msg = "保存成功", Data = null });
        //            }
        //        }
        //        return Json(new UserSsoValidation<Users> { Rst = false, Msg = "应用信息错误，请检查AppId信息", Data = null });
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Error(e.Message);
        //        return Json(new UserSsoValidation<Users> { Rst = false, Msg = "应用执行错误", Data = null });
        //    }

        //}
        #endregion

    }
}