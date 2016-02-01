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
        public string chUserID { get; set; }
        
        [StringLength(32, ErrorMessage="欄位長度不得大於 32 個字元")]
        [Required]
        public string chXData { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        public string chUserName { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string chDeptName { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string chEMail { get; set; }
        
        [StringLength(7, ErrorMessage="欄位長度不得大於 7 個字元")]
        public string chEndDate { get; set; }
        public Nullable<System.DateTime> dtEndDate { get; set; }
        public Nullable<System.DateTime> dtLastModified { get; set; }
        
        [StringLength(5, ErrorMessage="欄位長度不得大於 5 個字元")]
        public string chXDataHosp { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string chUserType { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        public string chUserID10 { get; set; }
    }
}
