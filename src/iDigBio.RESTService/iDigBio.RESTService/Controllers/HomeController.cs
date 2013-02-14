using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDigBio.RESTService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Test"]) && Request.QueryString["Test"].Equals("basic", StringComparison.InvariantCultureIgnoreCase)) 
                return View("APITestBasic");
            else 
                return View();
        }
    }
}
