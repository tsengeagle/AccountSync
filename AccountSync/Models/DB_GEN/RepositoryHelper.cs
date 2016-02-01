namespace AccountSync.Models.DB_GEN
{
	public static class RepositoryHelper
	{
		public static IUnitOfWork GetUnitOfWork()
		{
			return new EFUnitOfWork();
		}		
		
		public static GenProxyAccountRepository GetGenProxyAccountRepository()
		{
			var repository = new GenProxyAccountRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static GenProxyAccountRepository GetGenProxyAccountRepository(IUnitOfWork unitOfWork)
		{
			var repository = new GenProxyAccountRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		
	}
}