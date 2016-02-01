using System;
using System.Linq;
using System.Collections.Generic;
	
namespace AccountSync.Models.hluser
{   
	public  class passwdRepository : EFRepository<passwd>, IpasswdRepository
	{

	}

	public  interface IpasswdRepository : IRepository<passwd>
	{

	}
}