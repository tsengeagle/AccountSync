using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountSync.Controllers
{
    public class UserAccountController : Controller
    {
        Models.hluserEntities hluser = new Models.hluserEntities();

        // GET: UserAccount
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Query(string user)
        {
            var MyAccount = hluser.passwd.Find(user.ToUpper());
            if (MyAccount == null)
            {
                return View("AccountNotFind");
            }
            else
            {
                return View(MyAccount);
            }
        }

        public ActionResult ResetPassword(string user)
        {
            var resetVM = hluser.passwd.Where(p => p.user == user.ToUpper()).Select(s => new ResetPasswordViewModel() { User = s.user, Name = s.fullname }).First();
            if (resetVM == null)
            {
                return View("AccountNotFind");
            }
            else
            {
                return View(resetVM);
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel resetVM)
        {
            Models.HIS3_DB_MGTEntities hisdb = new Models.HIS3_DB_MGTEntities();
            string oldPasswordMD5String = hisdb.GetMD5(resetVM.OldPassword).First();

            var user = hluser.passwd.Find(resetVM.User);

            if (user == null)
            {
                return View("AccountNotFind");
            }
            else
            {
                if (user.password!=oldPasswordMD5String.ToLower())
                {
                    return View("PasswordIncorrect", resetVM);

                }
                else
                {
                    user.password = hisdb.GetMD5(resetVM.NewPassword).First().ToLower();
                    hluser.SaveChanges();
                    return View("PasswordChanged", resetVM);
                }
            }
        }

        public ActionResult ForgetPassword(string user)
        {
            var MyAccount = hluser.passwd.Find(user.ToUpper());
            if (MyAccount == null)
            {
                return View("AccountNotFind");
            }
            else
            {
                Models.HIS3_DB_MGTEntities hisdb = new Models.HIS3_DB_MGTEntities();
                string newPassword = hisdb.GetMD5(user).First();
                MyAccount.password = newPassword.ToLower();
                hluser.SaveChanges();

                return View("PasswordReseted", new ResetPasswordViewModel() {User=MyAccount.user,Name=MyAccount.fullname });
            }

        }
    }
}