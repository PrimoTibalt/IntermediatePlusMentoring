namespace DAL.Orders.Repository
{
    public interface ICartRepository : IGenericRepository<Cart, Guid>
    {
        Task<IList<CartItem>> GetItemsFull(Guid id);
        Task<IList<CartItem>> GetItemsWithEventSeat(Guid id);
    }
}