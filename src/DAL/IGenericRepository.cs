namespace DAL
{
	public interface IGenericRepository<TEntity, TKey> : ISaveUpdateRepository<TEntity>
		where TEntity : class
		where TKey : struct
	{
		Task<IList<TEntity>> GetAll();
		Task<TEntity> GetById(TKey id);
		Task Create(TEntity entity);
		Task Delete(TKey id);
	}
}
