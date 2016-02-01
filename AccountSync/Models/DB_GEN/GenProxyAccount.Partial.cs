namespace AccountSync.Models.DB_GEN
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(GenProxyAccountMetaData))]
    public partial class GenProxyAccount
    {
    }
    
    public partial class GenProxyAccountMetaData
    {
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        [Required]
        [Display(Name="上網帳號")]
        public string chUserID { get; set; }
        
        [StringLength(32, ErrorMessage="欄位長度不得大於 32 個字元")]
        [Required]
        [Display(Name = "密碼（已加密）")]
        public string chXData { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        [Display(Name = "姓名")]
        [Required]
        public string chUserName { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Display(Name = "單位")]
        public string chDeptName { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Display(Name = "Notes ID")]
        public string chEMail { get; set; }
        
        [StringLength(7, ErrorMessage="欄位長度不得大於 7 個字元")]
        [Display(Name = "停用日")]
        public string chEndDate { get; set; }
        [Display(Name = "帳號有效時間")]
        [Required]
        public Nullable<System.DateTime> dtEndDate { get; set; }
        [Display(Name = "最後修改時間")]
        public Nullable<System.DateTime> dtLastModified { get; set; }
        
        [StringLength(5, ErrorMessage="欄位長度不得大於 5 個字元")]
        [Display(Name = "密碼來源院區")]
        public string chXDataHosp { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Display(Name = "帳號類別")]
        public string chUserType { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        [Display(Name = "使用者ID")]
        public string chUserID10 { get; set; }
    }
}
