using Entities.Events;
using Entities.Notifications;
using Entities.Orders;
using Entities.Payments;
using Microsoft.EntityFrameworkCore;

namespace DAL.Payments
{
	internal sealed class PaymentContext : DbContext
	{
		public PaymentContext(DbContextOptions options) : base(options) { }

		public DbSet<NotificationEntity> Notifications { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Price> Prices { get; set; }
		public DbSet<EventSeat> EventSeats { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Event> Events { get; set; }
	}
}
