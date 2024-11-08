using DAL.Orders;

namespace OrderApplication.Entities
{
	public class CartDetails
	{
		public IList<CartItemDetails> Items { get; set; }
		public decimal TotalAmount => Items.Sum(i => i.Price);
	}
}