using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountSync.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "ProxyAccount");
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MD5Test()
        {
            Models.HIS3_DB_MGTEntities db = new Models.HIS3_DB_MGTEntities();

            string myMD5 = db.GetMD5("password").First();
            ViewData.Add("md5", myMD5);
            return View();
        }

    }
}