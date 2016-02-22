using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AccountSync.Models
{
    public class AccountParameterViewModel
    {
        [Display(Name="依帳號")]
        public string ByUserID { get; set; }
        [Display(Name = "依姓名")]
        public string ByUserName { get; set; }
        [Display(Name = "依單位")]
        public string ByDeptName { get; set; }


        
    }
}