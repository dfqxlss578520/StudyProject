using Hyl.Core.Configuration;
using Hyl.Core.Helpers.Utility;
using Hyl.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyl.Survey.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class HylCustomerAuthorizeFilter : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            HylWebConfig _cfg = EngineContext.Current.Resolve<HylWebConfig>();
            return _cfg.IntegrateGuanKong ? (int)WebWorkContext.AuthorityId <= 100 : true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext != null)
            {
                var url = filterContext.HttpContext.Request.RawUrl;
                filterContext.HttpContext.Response.Redirect(string.Format("/Proprietor/Auth/Index?rurl={0}", Utils.UrlEncode(url)));
            }
            else
            {
                throw new ArgumentNullException("filterContext");
            }
        }
    }
}