using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ulaznicar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Svrha i ideja iza Ulazničara";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt na koji se možete javiti s pitanjima.";

            return View();
        }
    }
}