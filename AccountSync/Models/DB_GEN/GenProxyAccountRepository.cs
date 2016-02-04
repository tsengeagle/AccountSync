using System;
using System.Linq;
using System.Collections.Generic;
	
namespace AccountSync.Models.DB_GEN
{   
	public  class GenProxyAccountRepository : EFRepository<GenProxyAccount>, IGenProxyAccountRepository
	{
        internal GenProxyAccount GetUser(string UserID)
        {
            return this.Where(w => w.chUserID == UserID.Trim()).FirstOrDefault();
        }

        internal ChangePasswordViewModel GetChangePasswordVM(string UserID)
        {
            var user = this.GetUser(UserID);
            return new ChangePasswordViewModel()
            {
                UserID = user.chUserID,
                UserName = user.chUserName,
                OldPassword = "",
                NewPassword = "",
                NewPasswordConfirm = ""
            };
        }

        internal string GetMD5(string p)
        {
            DB_GENEntities DB_GEN = new DB_GENEntities();
            return DB_GEN.GetMD5(p).First().ToUpper();
        }


        internal List<string> GetDeptList()
        {
            return this.All().Select(s => s.chDeptName).Distinct().ToList();
        }
    }

	public  interface IGenProxyAccountRepository : IRepository<GenProxyAccount>
	{

	}
}