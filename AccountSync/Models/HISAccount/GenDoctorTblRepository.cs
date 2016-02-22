using System;
using System.Linq;
using System.Collections.Generic;
	
namespace AccountSync.Models.HISAccount
{   
	public  class GenDoctorTblRepository : EFRepository<GenDoctorTbl>, IGenDoctorTblRepository
	{

	}

	public  interface IGenDoctorTblRepository : IRepository<GenDoctorTbl>
	{

	}
}