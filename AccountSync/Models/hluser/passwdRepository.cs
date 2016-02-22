using System;
using System.Linq;
using System.Collections.Generic;
	
namespace AccountSync.Models.hluser
{   
	public  class passwdRepository : EFRepository<passwd>, IpasswdRepository
	{

        internal passwd GetUser(string p)
        {
            return this.Where(w => w.user == p.Trim()).FirstOrDefault();
        }
    }

	public  interface IpasswdRepository : IRepository<passwd>
	{

	}
}