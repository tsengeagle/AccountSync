using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountSync.Models
{
    public partial class NewAccountViewModel
    {
        [Display(Name = "名稱")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "上網帳號")]
        [Required]
        public string UserID { get; set; }
        [Display(Name = "單位")]
        public string DeptName { get; set; }
        [Display(Name = "Notes ID")]
        public string NoteID { get; set; }
        [Display(Name = "帳號有效期限")]
        [Required]
        public DateTime? dtEndDate { get; set; }

    }
}