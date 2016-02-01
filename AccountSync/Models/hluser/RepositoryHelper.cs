namespace AccountSync.Models.hluser
{
	public static class RepositoryHelper
	{
		public static IUnitOfWork GetUnitOfWork()
		{
			return new EFUnitOfWork();
		}		
		
		public static passwdRepository GetpasswdRepository()
		{
			var repository = new passwdRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static passwdRepository GetpasswdRepository(IUnitOfWork unitOfWork)
		{
			var repository = new passwdRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		
	}
}