using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hyl.Core.Infrastructure;
using StackExchange.Profiling;

namespace Hyl.Survey
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //disable "X-AspNetMvc-Version" header name
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            EngineContext.Initialize(false);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsLocal)
            {
                MiniProfiler.Stop();
            }
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
