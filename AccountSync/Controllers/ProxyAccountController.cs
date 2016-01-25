using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace AccountSync.Controllers
{
    public class ProxyAccountController : Controller
    {
        Models.DB_GENEntities DB_GEN = new Models.DB_GENEntities();
        Models.hluserEntities hluser = new Models.hluserEntities();
        Models.MedProxyEntities MedProxy = new Models.MedProxyEntities();

        int pageSize = 15;

        // GET: ProxyAccount
        public ActionResult Index(int page = 1)
        {
            return RedirectToAction("Query");

            //int currentPage = page < 1 ? 1 : page;

            //var myAccount = DB_GEN.GenProxyAccount.OrderBy(o => o.chUserID).ToArray();
            //var proxyAccount = hluser.passwd.OrderBy(o => o.user).ToArray();

            //var result = from his in myAccount
            //             join mysql in proxyAccount
            //             on his.chUserID equals mysql.user
            //             select new Models.ProxyAccountViewModel()
            //             {
            //                 UserID = his.chUserID,
            //                 UserName = his.chUserName,
            //                 DeptName = his.chDeptName,
            //                 dtEndDate = his.dtEndDate,
            //                 NoteID = his.chEMail,
            //                 isSynced = his.chXData.ToLower() == mysql.password.ToLower() ? true : false,
            //                 isEnabled = !mysql.enabled
            //             };

            //return View(result.ToPagedList(currentPage, pageSize));
        }

        public ActionResult Query()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Query(string UserID)
        {
            var myAccount = DB_GEN.GenProxyAccount.Find(UserID);
            if (myAccount == null)
            {
                return View("AccountNotFound");
            }
            return View("QueryResult", myAccount);
        }

        public ActionResult ChangePassword(string UserID)
        {
            var changePasswordVM = DB_GEN.GenProxyAccount.Where(u => u.chUserID == UserID).
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

            var myAccount = DB_GEN.GenProxyAccount.Find(model.UserID.Trim());
            var myProxyAccount = hluser.passwd.Find(model.UserID.Trim());
            var myMedProxyAccount = MedProxy.passwd.Find(model.UserID.Trim());

            if ((myAccount == null) || (myProxyAccount == null) || (myMedProxyAccount == null))
            {
                return View("AccountNotFound");
            }

            string OldPasswordMD5 = DB_GEN.GetMD5(model.OldPassword).First().ToUpper();

            if (myAccount.chXData != OldPasswordMD5)
            {
                return View("PasswordIncorrect");
            }

            if (model.NewPassword != model.NewPasswordConfirm)
            {
                return View("PasswordInconfirm");
            }

            string NewPasswordMD5 = DB_GEN.GetMD5(model.NewPassword).First().ToUpper();
            myAccount.chXData = NewPasswordMD5;
            myAccount.dtLastModified = DateTime.Now;
            myAccount.chXDataHosp = "Web";
            DB_GEN.SaveChanges();

            myProxyAccount.password = NewPasswordMD5.ToLower();
            myProxyAccount.comment = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "; Update by web";
            hluser.SaveChanges();

            myMedProxyAccount.password = NewPasswordMD5.ToLower();
            myMedProxyAccount.comment = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "; Update by web";
            MedProxy.SaveChanges();

            return View("PasswordChanged");
        }

        public ActionResult ResetPassword(string UserID)
        {
            //TODO 為了相容於六碼ID，所以目前所有UserID都有先trim過，後面有空要改為repo樣式來統一整個邏輯
            var myAccount = DB_GEN.GenProxyAccount.Find(UserID.Trim());
            if (myAccount == null)
            {
                return View("AccountNotFound");
            }

            Random rand = new Random(DateTime.Now.Millisecond);
            string randPassword = "MF" + Convert.ToString(rand.Next(10000000, 99999999)).Substring(0, 4);
            ViewData.Add("NewPassword", randPassword);

            string NewPasswordMD5 = DB_GEN.GetMD5(randPassword).First().ToUpper();
            myAccount.chXData = NewPasswordMD5;
            myAccount.dtLastModified = DateTime.Now;
            myAccount.chXDataHosp = "Web";

            DB_GEN.SaveChanges();

            var myProxyAccount = hluser.passwd.Find(UserID.Trim());
            if (myProxyAccount == null)
            {
                return View("AccountNotFound");
            }

            myProxyAccount.password = NewPasswordMD5.ToLower();
            myProxyAccount.comment = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "; Reset by web";
            hluser.SaveChanges();

            var myMedProxyAccount = MedProxy.passwd.Find(UserID.Trim());
            if (myMedProxyAccount==null)
            {
                return View("AccountNotFound");
            }
            myMedProxyAccount.password = NewPasswordMD5.ToLower();
            myMedProxyAccount.comment = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "; Reset by web";
            MedProxy.SaveChanges();

            return View("PasswordReseted", myAccount);
        }
    }


}