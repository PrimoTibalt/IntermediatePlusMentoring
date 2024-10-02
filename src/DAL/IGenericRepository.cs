namespace DAL
{
	public interface IGenericRepository<TEntity, TKey>
		where TEntity : class
		where TKey : struct
	{
		Task<IList<TEntity>> GetAll();
		Task<TEntity> GetById(TKey id);
		void Create(TEntity entity);
		void Update(TEntity entity);
		Task Delete(TKey id);
		Task<int> Save();
	}
}
