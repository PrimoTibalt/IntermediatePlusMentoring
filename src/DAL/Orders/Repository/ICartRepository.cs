namespace DAL.Orders.Repository
{
    public interface ICartRepository : IGenericRepository<Cart, Guid>
    {
        Task<IList<CartItem>> GetItems(Guid id);
    }
}