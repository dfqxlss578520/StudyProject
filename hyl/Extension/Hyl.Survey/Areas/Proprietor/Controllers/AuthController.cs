using Hyl.Core.Configuration;
using Hyl.Survey.Infrastructure;
using Hyl.Survey.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hyl.Survey.Areas.Proprietor.Controllers
{
    public class AuthController : Controller
    {
        private HylWebConfig _hylWebConfig;
        public AuthController(HylWebConfig hylWebConfig)
        {
            _hylWebConfig = hylWebConfig;
            if (!string.IsNullOrEmpty(_hylWebConfig.GkUrl))
            {
                _hylWebConfig.GkUrl = _hylWebConfig.GkUrl.Trim('/');
            }
        }
        public ActionResult Index(string rurl)
        {
            ViewBag.rurl = rurl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string phone, string rurl = "")
        {
            if (!string.IsNullOrEmpty(phone))
            {
                HttpClient client = new HttpClient();
                var httpResponseMessage = await client.PostAsync(_hylWebConfig.GkUrl + string.Format("/Survey/Handler/Dept.ashx?action=10&phone={0}", phone), null);
                var customerJson = await httpResponseMessage.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(customerJson))
                {
                    var customer = JsonConvert.DeserializeObject<CustomerLoginViewModel>(customerJson);
                    if (customer != null && customer.rst && customer.Data.Count > 0)
                    {
                        WebWorkContext.CustomerUser = customer.DefaultModel;
                        WebWorkContext.AuthorityId = (int)AuthorityEnum.Customer;
                        if (!string.IsNullOrEmpty(rurl))
                        {
                            return Redirect(rurl);
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                }
            }
            ViewBag.rurl = rurl;
            ViewBag.errorTip = "手机号错误，请检查";
            return View();
        }

        public ActionResult LoginOut()
        {
            WebWorkContext.CustomerLoginOut();
            return RedirectToAction("Index");
        }

    }
}