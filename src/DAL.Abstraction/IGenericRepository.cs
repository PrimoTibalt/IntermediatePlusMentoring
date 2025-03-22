namespace DAL.Abstraction
{
	public interface IGenericRepository<TEntity, TKey> : ISaveUpdateRepository<TEntity>
		where TEntity : class
		where TKey : struct
	{
		Task<IList<TEntity>> GetAll(CancellationToken token = default);
		Task<TEntity> GetById(TKey id);
		Task Create(TEntity entity);
		Task Delete(TKey id);
	}
}
