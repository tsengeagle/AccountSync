﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountSync.Models.EMail
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class EMailEntities : DbContext
    {
        public EMailEntities()
            : base("name=EMailEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int SendMail(string chSendTo, string chCopyTo, string chBlindCopyTo, string chSubjectT, string chBody)
        {
            var chSendToParameter = chSendTo != null ?
                new ObjectParameter("chSendTo", chSendTo) :
                new ObjectParameter("chSendTo", typeof(string));
    
            var chCopyToParameter = chCopyTo != null ?
                new ObjectParameter("chCopyTo", chCopyTo) :
                new ObjectParameter("chCopyTo", typeof(string));
    
            var chBlindCopyToParameter = chBlindCopyTo != null ?
                new ObjectParameter("chBlindCopyTo", chBlindCopyTo) :
                new ObjectParameter("chBlindCopyTo", typeof(string));
    
            var chSubjectTParameter = chSubjectT != null ?
                new ObjectParameter("chSubjectT", chSubjectT) :
                new ObjectParameter("chSubjectT", typeof(string));
    
            var chBodyParameter = chBody != null ?
                new ObjectParameter("chBody", chBody) :
                new ObjectParameter("chBody", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SendMail", chSendToParameter, chCopyToParameter, chBlindCopyToParameter, chSubjectTParameter, chBodyParameter);
        }
    }
}