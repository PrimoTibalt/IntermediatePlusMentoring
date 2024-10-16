using DAL.Orders;

namespace OrderApplication.Entities
{
    public class CartDetails
    {
        public IList<CartItem> Items { get; set; }
        public decimal TotalAmount => Items.Sum(i => i.Price.Sum);
    }
}