﻿using DAL.Orders;

namespace DAL.Payments
{
	public sealed class Payment
	{
		public long Id { get; set; }
		public Guid CartId { get; set; }
		public Cart Cart { get; set; }
		public int Status { get; set; }
	}
}
