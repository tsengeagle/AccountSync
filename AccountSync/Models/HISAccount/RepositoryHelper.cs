namespace AccountSync.Models.HISAccount
{
	public static class RepositoryHelper
	{
		public static IUnitOfWork GetUnitOfWork()
		{
			return new EFUnitOfWork();
		}		
		
		public static GenDoctorTblRepository GetGenDoctorTblRepository()
		{
			var repository = new GenDoctorTblRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static GenDoctorTblRepository GetGenDoctorTblRepository(IUnitOfWork unitOfWork)
		{
			var repository = new GenDoctorTblRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static GenUserProfile1Repository GetGenUserProfile1Repository()
		{
			var repository = new GenUserProfile1Repository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static GenUserProfile1Repository GetGenUserProfile1Repository(IUnitOfWork unitOfWork)
		{
			var repository = new GenUserProfile1Repository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		
	}
}