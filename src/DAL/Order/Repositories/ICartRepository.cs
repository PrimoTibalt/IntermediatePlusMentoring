namespace DAL.Order.Repositories
{
	public interface ICartRepository : IGenericRepository<Cart>
	{
		Task<Cart> GetById(Guid id);
	}
}
