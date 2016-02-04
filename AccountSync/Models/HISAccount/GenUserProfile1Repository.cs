using System;
using System.Linq;
using System.Collections.Generic;
	
namespace AccountSync.Models.HISAccount
{   
	public  class GenUserProfile1Repository : EFRepository<GenUserProfile1>, IGenUserProfile1Repository
	{

	}

	public  interface IGenUserProfile1Repository : IRepository<GenUserProfile1>
	{

	}
}