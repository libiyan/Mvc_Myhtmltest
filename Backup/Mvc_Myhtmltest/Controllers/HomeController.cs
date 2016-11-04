using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_Myhtmltest.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult pianyi()
        {
            return View();
        }

        public ActionResult CrossDomain_A(){

            return View();
        }

        public ActionResult CrossDomain_B()
        {

            return View();
        }

        public ActionResult CrossDomain_proxy()
        {

            return View();
        }




    }
}
