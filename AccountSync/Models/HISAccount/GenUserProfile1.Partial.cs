namespace AccountSync.Models.HISAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(GenUserProfile1MetaData))]
    public partial class GenUserProfile1
    {
    }
    
    public partial class GenUserProfile1MetaData
    {
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        [Required]
        public string chUserID { get; set; }
        
        [StringLength(6, ErrorMessage="欄位長度不得大於 6 個字元")]
        public string chUserPass { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        public string chUserName { get; set; }
        
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string chUserOPD { get; set; }
        
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string chUserAMD { get; set; }
        
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string chUserMrbasic { get; set; }
        
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string chUserMeg { get; set; }
        
        [StringLength(3, ErrorMessage="欄位長度不得大於 3 個字元")]
        public string chUserGro { get; set; }
        
        [StringLength(7, ErrorMessage="欄位長度不得大於 7 個字元")]
        public string chStartDate { get; set; }
        
        [StringLength(7, ErrorMessage="欄位長度不得大於 7 個字元")]
        public string chEndDate { get; set; }
        
        [StringLength(13, ErrorMessage="欄位長度不得大於 13 個字元")]
        public string chLogInDT { get; set; }
        
        [StringLength(13, ErrorMessage="欄位長度不得大於 13 個字元")]
        public string chLogOutDT { get; set; }
        
        [StringLength(20, ErrorMessage="欄位長度不得大於 20 個字元")]
        public string chLogInStat { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        public string chUserSector { get; set; }
        
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string chUserAcc { get; set; }
        
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string chUserIPH { get; set; }
        
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string chUserRad { get; set; }
        
        [StringLength(11, ErrorMessage="欄位長度不得大於 11 個字元")]
        public string chOffStartDate { get; set; }
        
        [StringLength(11, ErrorMessage="欄位長度不得大於 11 個字元")]
        public string chOffEndDate { get; set; }
        
        [StringLength(32, ErrorMessage="欄位長度不得大於 32 個字元")]
        public string chXData { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        public string chRandomKey { get; set; }
        
        [StringLength(20, ErrorMessage="欄位長度不得大於 20 個字元")]
        public string chUserOtherKey { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        public string chLastModifyID { get; set; }
        public Nullable<System.DateTime> sdLastModifyDate { get; set; }
    }
}
