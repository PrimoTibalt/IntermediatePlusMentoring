namespace DAL
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		Task<IList<TEntity>> GetAll();
		Task<TEntity> GetById(int id);
		void Create(TEntity entity);
		void Update(TEntity entity);
		Task Delete(int id);
		Task<int> Save();
	}
}
