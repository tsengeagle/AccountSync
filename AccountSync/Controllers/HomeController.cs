using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AccountSync.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Models.LoginViewModel model = new Models.LoginViewModel() { ReturnURL = returnUrl };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Models.LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            FormsAuthentication.RedirectFromLoginPage(model.UserID, false);

            return View();
        }
    }
}