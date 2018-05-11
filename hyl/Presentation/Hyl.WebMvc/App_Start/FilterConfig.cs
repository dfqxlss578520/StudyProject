using Hyl.Web.Framework.Filters;
using System.Web.Mvc;
using Hyl.Web.Framework.Authorization;

namespace Hyl.WebMvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new HylExceptionFilter());
            //filters.Add(new HylAuthorizeFilter());
        }
    }
}
