using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Hyl.Core.Domain.MemberCenters;
using Hyl.Core.Infrastructure;

namespace Hyl.SSO
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //disable "X-AspNetMvc-Version" header name
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            EngineContext.Initialize(false);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Users, UserValidateViewModel>();
            });
        }

        /// <summary>
        /// Remove Http Reponse Header Server varible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }
        }
    }
}
