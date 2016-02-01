namespace AccountSync.Models.hluser
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(passwdMetaData))]
    public partial class passwd
    {
    }
    
    public partial class passwdMetaData
    {
        
        [StringLength(32, ErrorMessage="欄位長度不得大於 32 個字元")]
        [Required]
        public string user { get; set; }
        
        [StringLength(35, ErrorMessage="欄位長度不得大於 35 個字元")]
        [Required]
        public string password { get; set; }
        [Required]
        public bool enabled { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string Dept { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string mail_address { get; set; }
        
        [StringLength(60, ErrorMessage="欄位長度不得大於 60 個字元")]
        public string fullname { get; set; }
        
        [StringLength(60, ErrorMessage="欄位長度不得大於 60 個字元")]
        public string comment { get; set; }
    }
}
