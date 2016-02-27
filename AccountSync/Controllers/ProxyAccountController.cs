using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace AccountSync.Controllers
{
    public class ProxyAccountController : Controller
    {
        AccountSync.Models.DB_GEN.GenProxyAccountRepository DB_GEN_Repo;
        AccountSync.Models.hluser.passwdRepository hluser_Repo;
        AccountSync.Models.hluser.passwdRepository MedProxy_Repo;

        AccountSync.Models.HISAccount.GenUserProfile1Repository HisAccountRepo;
        AccountSync.Models.HISAccount.GenDoctorTblRepository HisDoctorRepo;

        public ProxyAccountController()
        {
            DB_GEN_Repo = AccountSync.Models.DB_GEN.RepositoryHelper.GetGenProxyAccountRepository();

            hluser_Repo = AccountSync.Models.hluser.RepositoryHelper.GetpasswdRepository();

            AccountSync.Models.hluser.EFUnitOfWork medProxyUOW = new Models.hluser.EFUnitOfWork();
            medProxyUOW.ConnectionString = "server=10.2.100.10;user id=guid;password=gpwd;persistsecurityinfo=True;database=hluser";
            MedProxy_Repo = AccountSync.Models.hluser.RepositoryHelper.GetpasswdRepository(medProxyUOW);

        }

        // GET: ProxyAccount
        public ActionResult Index(int page = 1)
        {
            //return RedirectToAction("Query");

            return View();
        }

        //[AllowAnonymous]
        //public ActionResult Query()
        //{
        //    return RedirectToAction("Index");
        //}

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Query(string UserID)
        {
            var myAccount = DB_GEN_Repo.GetUser(UserID);// DB_GEN.GenProxyAccount.Find(UserID);
            if (myAccount == null)
            {
                return View("AccountNotFound");
            }

            string connToHis = "";
            switch (myAccount.chXDataHosp.Trim())
            {
                case "HL":
                    connToHis = "server=HLOPDSQL;user id=guid;password=gpwd;persistsecurityinfo=True;database=DB_GEN";
                    break;
                case "DL":
                    connToHis = "server=DLOPDSQL;user id=guid;password=gpwd;persistsecurityinfo=True;database=DB_GEN";
                    break;
                case "XD":
                    connToHis = "server=OPDSQL1;user id=guid;password=gpwd;persistsecurityinfo=True;database=DB_GEN";
                    break;
                case "TC":
                    connToHis = "server=TCOPDSQL;user id=guid;password=gpwd;persistsecurityinfo=True;database=DB_GEN";
                    break;
                case "GS":
                    connToHis = "server=GSSVR;user id=guid;password=gpwd;persistsecurityinfo=True;database=DB_GEN";
                    break;
                case "UL":
                    connToHis = "server=ULSVR;user id=guid;password=gpwd;persistsecurityinfo=True;database=DB_GEN";
                    break;
                case "TL":
                    connToHis = "server=TLSVR;user id=guid;password=gpwd;persistsecurityinfo=True;database=DB_GEN";
                    break;
                default:
                    connToHis = "";
                    break;
            }

            Models.AccountDetailViewModel detail = new Models.AccountDetailViewModel();
            detail.chUserID = myAccount.chUserID;
            detail.chUserName = myAccount.chUserName;
            detail.chDeptName = myAccount.chDeptName;
            detail.chEMail = myAccount.chEMail;
            detail.chEndDate = myAccount.chEndDate;
            detail.dtEndDate = myAccount.dtEndDate;
            detail.dtLastModified = myAccount.dtLastModified;
            detail.chXDataHosp = myAccount.chXDataHosp;
            detail.chUserType = myAccount.chUserType;
            detail.chXData = myAccount.chXData;
            detail.chUserID10 = myAccount.chUserID10;

            if (!string.IsNullOrWhiteSpace(connToHis))
            {
                AccountSync.Models.HISAccount.EFUnitOfWork hisUOW = new Models.HISAccount.EFUnitOfWork();
                hisUOW.ConnectionString = connToHis;
                HisAccountRepo = Models.HISAccount.RepositoryHelper.GetGenUserProfile1Repository(hisUOW);
                var hisUser = HisAccountRepo.Where(w => (w.chUserID == detail.chUserID10)).FirstOrDefault();
                if (hisUser == null)
                {
                    HisDoctorRepo = Models.HISAccount.RepositoryHelper.GetGenDoctorTblRepository(hisUOW);

                    //拉出所有醫師代碼
                    var hisDoctors = HisDoctorRepo.Where(w => (w.chIDNo == detail.chUserID10));
                    foreach (var doctor in hisDoctors)
                    {
                        var docAccount = new Models.HisAccountViewModel();
                        docAccount.chUserID = detail.chUserID;
                        docAccount.UserID = doctor.chDocNo;
                        docAccount.UserName = doctor.chDocName;
                        docAccount.isRightPassword = false;
                        docAccount.chXData = "Empty";
                        docAccount.chXDataHosp = detail.chXDataHosp;
                        var docResult = HisAccountRepo.Where(w => w.chUserID == doctor.chDocNo).FirstOrDefault();
                        if (docResult != null)
                        {
                            docAccount.chXData = docResult.chXData;
                            if (docAccount.chXData == detail.chXData)
                            {
                                docAccount.isRightPassword = true;
                            }
                            else
                            {
                                docAccount.isRightPassword = false;
                            }
                            detail.HisAccounts.Add(docAccount);
                        }
                    }

                }
                else
                {

                    var hisAccount = new Models.HisAccountViewModel();
                    hisAccount.chUserID = detail.chUserID;
                    hisAccount.UserID = "非醫師";
                    hisAccount.UserName = detail.chUserName;
                    hisAccount.isRightPassword = detail.chXData == hisUser.chXData ? true : false;
                    hisAccount.chXData = hisUser.chXData;
                    hisAccount.chXDataHosp = detail.chUserID;

                    detail.HisAccounts.Add(hisAccount);
                }
            }
            else
            {
                detail.chHisXData = "密碼不是來自特定院區";
            }
            //return View("AccountDetail", myAccount);
            return View("QueryResult", detail);
        }

        public ActionResult ChangePassword(string UserID)
        {
            //var changePasswordVM = DB_GEN.GenProxyAccount.Where(u => u.chUserID == UserID).
            //    Select(s => new Models.ChangePasswordViewModel()
            //    {
            //        UserID = s.chUserID,
            //        UserName = s.chUserName,
            //        OldPassword = "",
            //        NewPassword = "",
            //        NewPasswordConfirm = ""
            //    }).
            //    First();

            var changePasswordVM = DB_GEN_Repo.GetChangePasswordVM(UserID);

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

            var myAccount = DB_GEN_Repo.GetUser(model.UserID); // DB_GEN.GenProxyAccount.Find(model.UserID.Trim());
            var myProxyAccount = hluser_Repo.GetUser(model.UserID); // hluser.passwd.Find(model.UserID.Trim());
            var myMedProxyAccount = MedProxy_Repo.GetUser(model.UserID); // MedProxy.passwd.Find(model.UserID.Trim());

            if ((myAccount == null) || (myProxyAccount == null) || (myMedProxyAccount == null))
            {
                return View("AccountNotFound");
            }

            string OldPasswordMD5 = DB_GEN_Repo.GetMD5(model.OldPassword); // DB_GEN.GetMD5(model.OldPassword).First().ToUpper();

            if (myAccount.chXData != OldPasswordMD5)
            {
                return View("PasswordIncorrect");
            }

            if (model.NewPassword != model.NewPasswordConfirm)
            {
                return View("PasswordInconfirm");
            }

            string NewPasswordMD5 = DB_GEN_Repo.GetMD5(model.NewPassword); //DB_GEN.GetMD5(model.NewPassword).First().ToUpper();
            myAccount.chXData = NewPasswordMD5;
            myAccount.dtLastModified = DateTime.Now;
            myAccount.chXDataHosp = "Web";
            DB_GEN_Repo.UnitOfWork.Commit(); //DB_GEN.SaveChanges();

            myProxyAccount.password = NewPasswordMD5.ToLower();
            myProxyAccount.comment = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "; Update by web";
            hluser_Repo.UnitOfWork.Commit(); //hluser.SaveChanges();

            myMedProxyAccount.password = NewPasswordMD5.ToLower();
            myMedProxyAccount.comment = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "; Update by web";
            MedProxy_Repo.UnitOfWork.Commit(); // MedProxy.SaveChanges();

            return View("PasswordChanged");
        }


        public ActionResult ResetPassword(string UserID)
        {
            //TODO 為了相容於六碼ID，所以目前所有UserID都有先trim過，後面有空要改為repo樣式來統一整個邏輯
            var myAccount = DB_GEN_Repo.GetUser(UserID); // DB_GEN.GenProxyAccount.Find(UserID.Trim());
            if (myAccount == null)
            {
                return View("AccountNotFound");
            }

            if (myAccount.chEMail.Trim().Length == 0)
            {
                TempData["Message"] = string.Format("上網帳號{0}的信箱地址為空白，無法寄發新密碼通知信，取消密碼重置，請洽詢資訊室更正信箱地址",myAccount.chUserID);
                return View("EMailEmpty");
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

            return View("PasswordReseted",myAccount);
        }

        public ActionResult QueryAccountList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CorrectionProxyPassword(Models.HisAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            try
            {
                var GenAccount = DB_GEN_Repo.Where(w => w.chUserID == model.chUserID).FirstOrDefault();
                if (GenAccount != null)
                {
                    GenAccount.chXData = model.chXData;
                    GenAccount.chXDataHosp = model.chXDataHosp;
                    GenAccount.dtLastModified = DateTime.Now;
                    DB_GEN_Repo.UnitOfWork.Commit();
                }
                return View("PasswordChanged");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }


}