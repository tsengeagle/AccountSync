using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AccountSync.Models
{
    [MetadataType(typeof(GenProxyAccountMetadata))]
    public partial class GenProxyAccount
    {

    }
    public partial class GenProxyAccountMetadata
    {
        [Display(Name="帳號")]
        public string chUserID { get; set; }
        [Display(Name = "密碼")]
        public string chXData { get; set; }
        [Display(Name = "姓名")]
        public string chUserName { get; set; }
        [Display(Name = "單位")]
        public string chDeptName { get; set; }
        [Display(Name = "Notes ID")]
        public string chEMail { get; set; }
        [Display(Name = "停用日")]
        public string chEndDate { get; set; }
        [Display(Name = "帳號有效期限")]
        public Nullable<System.DateTime> dtEndDate { get; set; }
        [Display(Name = "最後修改時間")]
        public Nullable<System.DateTime> dtLastModified { get; set; }
        [Display(Name = "密碼來源院區")]
        public string chXDataHosp { get; set; }
        [Display(Name = "帳號類別")]
        public string chUserType { get; set; }


    }
}