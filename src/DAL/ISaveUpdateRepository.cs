namespace DAL
{
	public interface ISaveUpdateRepository<TEntity> where TEntity : class
	{
		Task<int> Save();
		void Update(TEntity entity);
	}
}
