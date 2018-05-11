using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyl.Survey.Areas.Account.Controllers
{
    public class SurveyAuthorityController : AccountBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}