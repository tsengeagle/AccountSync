using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace AccountSync.Controllers
{
    public class ProxyManagementController : Controller
    {
        AccountSync.Models.DB_GEN.GenProxyAccountRepository DB_GEN_Repo;
        AccountSync.Models.hluser.passwdRepository hluser_Repo;

        public ProxyManagementController()
        {
            DB_GEN_Repo = Models.DB_GEN.RepositoryHelper.GetGenProxyAccountRepository();
            hluser_Repo = Models.hluser.RepositoryHelper.GetpasswdRepository();
        }

        int pageSize = 20;

        // GET: ProxyManagement
        public ActionResult Index(int page = 1)
        {

            int currentPage = page < 1 ? 1 : page;

            var myAccount = DB_GEN_Repo.All().OrderBy(o => o.chUserID).ToArray();  // DB_GEN.GenProxyAccount.OrderBy(o => o.chUserID).ToArray();
            var proxyAccount = hluser_Repo.All().OrderBy(o => o.user).ToArray(); // hluser.passwd.OrderBy(o => o.user).ToArray();

            var result = from his in myAccount
                         join mysql in proxyAccount
                         on his.chUserID equals mysql.user
                         select new Models.ProxyAccountViewModel()
                         {
                             UserID = his.chUserID,
                             UserName = his.chUserName,
                             DeptName = his.chDeptName,
                             dtEndDate = his.dtEndDate,
                             NoteID = his.chEMail,
                             isSynced = his.chXData.ToLower() == mysql.password.ToLower() ? true : false,
                             isEnabled = !mysql.enabled
                         };

            return View(result.ToPagedList(currentPage, pageSize));
            //var allList=
        }

        [Authorize]
        public ActionResult NewAccount()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewAccount(Models.NewAccountViewModel model)
        {
            //TODO 新增邏輯還沒弄完
            if (!ModelState.IsValid)
            {
                return View();
            }

            Models.DB_GEN.GenProxyAccount newAccount = new Models.DB_GEN.GenProxyAccount();
            newAccount.chUserID = model.UserID;
            newAccount.chUserName = model.UserName;
            newAccount.chDeptName = model.DeptName;
            newAccount.chEMail = model.NoteID;
            newAccount.dtEndDate = model.dtEndDate;
            newAccount.dtLastModified = DateTime.Now;
            newAccount.chXData = DB_GEN_Repo.GetMD5(model.UserID);
            newAccount.chXDataHosp = "Web";

            DB_GEN_Repo.Add(newAccount);

            DB_GEN_Repo.UnitOfWork.Commit();

            return View("AccountAdded", model);
        }

    }
}