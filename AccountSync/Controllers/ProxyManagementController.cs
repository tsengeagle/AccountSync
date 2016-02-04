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
    }
}