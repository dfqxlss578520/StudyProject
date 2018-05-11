using System;
using System.Web.Mvc;
using Hyl.Core.Infrastructure;
using Hyl.Core.Logs;

namespace Hyl.Web.Framework.Filters
{
    public class HylExceptionFilter : FilterAttribute, IExceptionFilter
    {
        private ILogs<HylExceptionFilter> _log;
        
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null)
            {
                if (filterContext.ExceptionHandled)
                {
                    return;
                }
                if (filterContext.Exception != null)
                {
                    _log = EngineContext.Current.Resolve<ILogs<HylExceptionFilter>>();
                    _log.Error(filterContext.Exception.Message);
                    UrlHelper url = new UrlHelper(filterContext.RequestContext);
                    filterContext.Result = new RedirectResult(url.Action("AboutError", "Home"));
                    filterContext.ExceptionHandled = true;
                }
            }
            else
            {
                throw new ArgumentNullException("filterContext");
            }
        }
    }
}
