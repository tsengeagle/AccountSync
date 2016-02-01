using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AccountSync.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "姓名")]
        public string UserName { get; set; }
        [Display(Name = "上網帳號")]
        public string UserID { get; set; }
        [Display(Name = "舊密碼"), Required(ErrorMessage = "必須輸入舊密碼")]
        public string OldPassword { get; set; }
        [Display(Name = "新密碼"), Required(ErrorMessage = "必須輸入新密碼"),
        MinLength(8, ErrorMessage = "密碼至少要8個字"), RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,30}$", ErrorMessage = "密碼強度不足")]
        public string NewPassword { get; set; }
        [Display(Name = "新密碼確認"), Required(ErrorMessage = "請再輸入一次新密碼確認"),
        MinLength(8, ErrorMessage = "密碼至少要8個字"), RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,30}$", ErrorMessage = "密碼強度不足"), 
        Compare("NewPassword", ErrorMessage = "新密碼不一致")]
        public string NewPasswordConfirm { get; set; }
    }
}