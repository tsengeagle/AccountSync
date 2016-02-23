using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace AccountSync.Controllers
{
    public class ProxyManagementController : Controller
    {
        AccountSync.Models.DB_GEN.GenProxyAccountRepository DB_GEN_Repo;
        AccountSync.Models.hluser.passwdRepository hluser_Repo;
        AccountSync.Models.hluser.passwdRepository MedProxy_Repo;

        public ProxyManagementController()
        {
            DB_GEN_Repo = Models.DB_GEN.RepositoryHelper.GetGenProxyAccountRepository();
            hluser_Repo = Models.hluser.RepositoryHelper.GetpasswdRepository();

            AccountSync.Models.hluser.EFUnitOfWork medProxyUOW = new Models.hluser.EFUnitOfWork();
            medProxyUOW.ConnectionString = "server=10.2.100.10;user id=guid;password=gpwd;persistsecurityinfo=True;database=hluser";
            MedProxy_Repo = AccountSync.Models.hluser.RepositoryHelper.GetpasswdRepository(medProxyUOW);
        }

        int pageSize = 20;

        // GET: ProxyManagement
        public ActionResult Index(int page = 1)
        {
            return RedirectToAction("AccountList");


            //int currentPage = page < 1 ? 1 : page;

            //var myAccount = DB_GEN_Repo.All().OrderBy(o => o.chUserID).ToArray();  // DB_GEN.GenProxyAccount.OrderBy(o => o.chUserID).ToArray();
            //var proxyAccount = hluser_Repo.All().OrderBy(o => o.user).ToArray(); // hluser.passwd.OrderBy(o => o.user).ToArray();

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

        public ActionResult ResetPassword(string UserID)
        {
            //TODO 為了相容於六碼ID，所以目前所有UserID都有先trim過，後面有空要改為repo樣式來統一整個邏輯
            var myAccount = DB_GEN_Repo.GetUser(UserID); // DB_GEN.GenProxyAccount.Find(UserID.Trim());
            if (myAccount == null)
            {
                return View("AccountNotFound");
            }

            Random rand = new Random(DateTime.Now.Millisecond);
            string randPassword = "MF" + Convert.ToString(rand.Next(10000000, 99999999)).Substring(0, 4);
            //ViewData.Add("NewPassword", randPassword);

            string NewPasswordMD5 = DB_GEN_Repo.GetMD5(randPassword); //DB_GEN.GetMD5(randPassword).First().ToUpper();
            myAccount.chXData = NewPasswordMD5;
            myAccount.dtLastModified = DateTime.Now;
            myAccount.chXDataHosp = "Web";

            DB_GEN_Repo.UnitOfWork.Commit(); // DB_GEN.SaveChanges();

            var myProxyAccount = hluser_Repo.GetUser(UserID); // hluser.passwd.Find(UserID.Trim());
            if (myProxyAccount == null)
            {
                return View("AccountNotFound");
            }

            myProxyAccount.password = NewPasswordMD5.ToLower();
            myProxyAccount.comment = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "; Reset by web";
            hluser_Repo.UnitOfWork.Commit(); // hluser.SaveChanges();

            var myMedProxyAccount = MedProxy_Repo.GetUser(UserID); // MedProxy.passwd.Find(UserID.Trim());
            if (myMedProxyAccount == null)
            {
                return View("AccountNotFound");
            }
            myMedProxyAccount.password = NewPasswordMD5.ToLower();
            myMedProxyAccount.comment = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "; Reset by web";
            MedProxy_Repo.UnitOfWork.Commit(); // MedProxy.SaveChanges();

            var mailContent = new StringBuilder();
            mailContent.AppendLine(string.Format("您的上網帳號:{0}", myAccount.chUserID));
            mailContent.AppendLine(string.Format("已於{0}重置密碼", DateTime.Now.ToString()));
            mailContent.AppendLine(string.Format("新密碼為:{0}", randPassword));
            mailContent.AppendLine(string.Format("請盡快至右方連結變更密碼: {0}", @"http://10.2.0.173/AccountSync/ProxyAccount"));
            
            var email = new Models.EMail.EMailEntities();
            email.SendMail(myAccount.chEMail, "", "", "密碼已重置", mailContent.ToString());

            return RedirectToAction("AccountList"); 

        }

        [Authorize]
        public ActionResult AccountList(int page = 1)
        {

            var ListViewModel = new Models.AccountPagedListViewModel();
            ListViewModel.CurrentPage = page < 1 ? 1 : page;
            ListViewModel.PageSize = ListViewModel.PageSize < 1 ? pageSize : ListViewModel.PageSize;

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

            ListViewModel.Accounts = result.OrderBy(o => o.UserID).ToPagedList(ListViewModel.CurrentPage, ListViewModel.PageSize);
            //ListViewModel.Accounts = DB_GEN_Repo.All().OrderBy(o => o.chUserID).ToPagedList(ListViewModel.CurrentPage, ListViewModel.PageSize);
            ListViewModel.Depts = new SelectList(DB_GEN_Repo.GetDeptListForDropDownList(), "<未選擇>");

            return View(ListViewModel);

        }

        [Authorize]
        [HttpPost]
        public ActionResult AccountList(Models.AccountPagedListViewModel model)
        {

            var result = DB_GEN_Repo.All();
            if (!string.IsNullOrWhiteSpace(model.Parameter.ByUserID))
            {
                result = result.Where(w => w.chUserID.Contains(model.Parameter.ByUserID));
            }
            if (!string.IsNullOrWhiteSpace(model.Parameter.ByUserName))
            {
                result = result.Where(w => w.chUserName.Contains(model.Parameter.ByUserName));
            }
            if ((!string.IsNullOrWhiteSpace(model.Parameter.ByDeptName)) && (model.Parameter.ByDeptName != "<未選擇>"))
            {
                result = result.Where(w => w.chDeptName.Contains(model.Parameter.ByDeptName));
            }

            var depts = result.Select(s => s.chDeptName).Distinct().ToList();
            depts.Add("<未選擇>");
            depts.Sort();
            model.Depts = new SelectList(depts, "<未選擇>");

            model.CurrentPage = model.CurrentPage < 1 ? 1 : model.CurrentPage;
            model.PageSize = model.PageSize < 1 ? pageSize : model.PageSize;

            var proxyAccount = MedProxy_Repo.All().OrderBy(o => o.user).ToArray(); // hluser.passwd.OrderBy(o => o.user).ToArray();
            var vm = from his in result.ToArray()
                     join mysql in proxyAccount
                     on his.chUserID.Trim() equals mysql.user.Trim()
                     into mysqlJoin
                     from mysql in mysqlJoin.DefaultIfEmpty()
                     select new Models.ProxyAccountViewModel()
                     {
                         UserID = his.chUserID,
                         UserName = his.chUserName,
                         DeptName = his.chDeptName,
                         dtEndDate = his.dtEndDate,
                         NoteID = his.chEMail,
                         isSynced = mysql == null ? false : his.chXData.ToLower() == mysql.password.ToLower() ? true : false,
                         isEnabled = mysql == null ? false : !mysql.enabled
                     };

            model.Accounts = vm.OrderBy(o => o.UserID).ToPagedList(model.CurrentPage, model.PageSize);

            return View(model);

        }
    }
}