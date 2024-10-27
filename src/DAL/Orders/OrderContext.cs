﻿using DAL.Events;
using DAL.Payments;
using DAL.Venues;
using Microsoft.EntityFrameworkCore;

namespace DAL.Orders
{
	public sealed class OrderContext : DbContext
	{
		public OrderContext(DbContextOptions options) : base(options) { }

		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<EventSeat> EventSeats { get; set; }
		public DbSet<Price> Prices { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Seat> Seats { get; set; }
		public DbSet<Row> Rows { get; set; }
	}
}
