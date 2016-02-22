using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountSync.Models
{
    public class AccountDetailViewModel
    {

        public string chUserID { get; set; }
        [Display(Name = "姓名")]
        public string chUserName { get; set; }
        [Display(Name = "單位")]
        public string chDeptName { get; set; }
        [Display(Name = "Notes ID")]
        public string chEMail { get; set; }
        [Display(Name = "停用日")]
        public string chEndDate { get; set; }
        [Display(Name = "停用時間")]
        public DateTime? dtEndDate { get; set; }
        [Display(Name = "最後修改時間")]
        public DateTime? dtLastModified { get; set; }
        [Display(Name = "密碼來源院區")]
        public string chXDataHosp { get; set; }
        [Display(Name = "使用者類別")]
        public string chUserType { get; set; }
        [Display(Name = "上網密碼")]
        public string chXData { get; set; }
        [Display(Name = "HIS密碼")]
        public string chHisXData { get; set; }
        public string chUserID10 { get; set; }
        [Display(Name = "HIS帳號")]
        public List<HisAccountViewModel> HisAccounts { get; set; }

        public AccountDetailViewModel()
        {
            HisAccounts = new List<HisAccountViewModel>();
        }

    }

    public class HisAccountViewModel
    {
        [Display(Name = "上網帳號")]
        public string chUserID { get; set; }
        [Display(Name = "密碼院區")]
        public string chXDataHosp { get; set; }

        [Display(Name = "HIS帳號")]
        public string UserID { get; set; }
        [Display(Name = "姓名")]
        public string UserName { get; set; }
        [Display(Name = "HIS密碼")]
        public string chXData { get; set; }
        [Display(Name = "是上網密碼？")]
        public bool isRightPassword { get; set; }
        public HisAccountViewModel()
        {
            isRightPassword = false;
            chXData = "Empty";
        }
    }
}