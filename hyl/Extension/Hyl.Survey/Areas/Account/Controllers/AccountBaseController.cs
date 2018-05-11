using System.Web.Mvc;
using Hyl.Survey.Infrastructure;

namespace Hyl.Survey.Areas.Account.Controllers
{
    [RouteArea("Account")]
    [HylSurveyMvcAuthorizeFilter]
    public class AccountBaseController : Controller
    {
        
    }
}