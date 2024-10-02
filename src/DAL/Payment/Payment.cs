﻿using DAL.Order;

namespace DAL.Payment
{
	public sealed class Payment
	{
		public long Id { get; set; }
		public Guid CartId { get; set; }
		public Cart Cart { get; set; }
		public string Status { get; set; }
	}
}
