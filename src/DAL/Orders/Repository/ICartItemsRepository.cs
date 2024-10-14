namespace DAL.Orders.Repository
{
    public interface ICartItemRepository : IGenericRepository<CartItem, long>
    {
       Task<CartItem> GetBy(Guid cartId, int eventId, long seatId); 
    }
}