using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AccountSync.Models
{
    public partial class ProxyAccountViewModel
    {
        [Display(Name = "名稱")]
        public string UserName { get; set; }
        [Display(Name = "上網帳號")]
        public string UserID { get; set; }
        [Display(Name = "單位")]
        public string DeptName { get; set; }
        [Display(Name = "Notes ID")]
        public string NoteID { get; set; }
        [Display(Name = "帳號有效期限")]
        public DateTime? dtEndDate { get; set; }
        [Display(Name = "密碼一致?")]
        public bool isSynced { get; set; }
        [Display(Name = "帳號可用?")]
        public bool isEnabled { get; set; }
    }
}