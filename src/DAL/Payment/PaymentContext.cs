using DAL.Events;
using DAL.Order;
using Microsoft.EntityFrameworkCore;

namespace DAL.Payment
{
	public sealed class PaymentContext : DbContext
	{
		public PaymentContext(DbContextOptions options) : base(options) { }

		public DbSet<Payment> Payments { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Price> Prices { get; set; }
		public DbSet<EventSeat> EventSeats { get; set; }
	}
}
