using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountSync.Controllers
{
    public class ProxyAccountController : Controller
    {
        Models.DB_GENEntities db = new Models.DB_GENEntities();

        // GET: ProxyAccount
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Query(string UserID)
        {
            var myAccount = db.GenProxyAccount.Find(UserID);
            if (myAccount == null)
            {
                return View("AccountNotFound");
            }
            return View(myAccount);
        }

        public ActionResult ChangePassword(string UserID)
        {
            var changePasswordVM = db.GenProxyAccount.Where(u => u.chUserID == UserID).
                Select(s => new Models.ChangePasswordViewModel()
                {
                    UserID = s.chUserID,
                    UserName = s.chUserName,
                    OldPassword = "",
                    NewPassword = "",
                    NewPasswordConfirm = ""
                }).
                First();

            if (changePasswordVM == null)
            {
                return View("AccountNotFound");
            }
            return View(changePasswordVM);
        }

        [HttpPost]
        public ActionResult ChangePassword(Models.ChangePasswordViewModel model)
        {
            if (model.OldPassword.Trim() == "")
            {
                return View(model);
            }

            var myAccount = db.GenProxyAccount.Find(model.UserID);
            if (myAccount == null)
            {
                return View("AccountNotFound");
            }

            string OldPasswordMD5 = db.GetMD5(model.OldPassword).First().ToUpper();

            if (myAccount.chXData != OldPasswordMD5)
            {
                return View("PasswordIncorrect");
            }

            if (model.NewPassword != model.NewPasswordConfirm)
            {
                return View("PasswordInconfirm");
            }

            string NewPasswordMD5 = db.GetMD5(model.NewPassword).First().ToUpper();
            myAccount.chXData = NewPasswordMD5;
            myAccount.dtLastModified = DateTime.Now;
            myAccount.chXDataHosp = "Web";

            db.SaveChanges();

            return View("PasswordChanged");
        }

        public ActionResult ResetPassword(string UserID)
        {
            var myAccount = db.GenProxyAccount.Find(UserID);
            if (myAccount == null)
            {
                return View("AccountNotFound");
            }

            string NewPasswordMD5 = db.GetMD5(myAccount.chUserID).First().ToUpper();
            myAccount.chXData = NewPasswordMD5;
            myAccount.dtLastModified = DateTime.Now;
            myAccount.chXDataHosp = "Web";

            db.SaveChanges();

            return View("PasswordReseted", myAccount);
        }
    }

 
}