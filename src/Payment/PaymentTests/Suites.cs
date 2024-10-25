using DAL.Events;
using DAL.Orders;
using DAL.Payments;
using TestsCore;

namespace PaymentTests
{
	public static class Suites
	{
		private static Dictionary<GetValuesSuites, Payment> suitePaymentMap => new()
		{
			{ GetValuesSuites.Empty, new() { Cart = new() { CartItems = [] } } },
			{ GetValuesSuites.OneValue, new() { Cart = new() { CartItems = suiteCartItemsMap[GetValuesSuites.OneValue]}} },
			{ GetValuesSuites.ManyValues, new() { Cart = new() { CartItems = suiteCartItemsMap[GetValuesSuites.ManyValues]}} },
		};

		/// <param name="seatStatus">
		/// null means that <see cref="EventSeat"/>.Status would be a
		/// random value of <see cref="SeatStatus"/> lower case string.
		/// </param>
		public static Payment GetPayment(GetValuesSuites suite, SeatStatus? seatStatus)
		{
			var payment = suitePaymentMap[suite];
			payment.Status = PaymentStatus.InProgress.ToString().ToLowerInvariant();
			// In true unit case for random values I would also add all possible values manually without random
			// random can be brutal / unit test should be repeatable!
			foreach (var item in payment.Cart.CartItems)
			{
				item.EventSeat = new();
				item.EventSeat.Status = seatStatus switch
				{
					SeatStatus.Booked => BookedStatus,
					SeatStatus.Available => AvailableStatus,
					SeatStatus.Sold => SoldStatus,
					null => new Random().Next(0, 3) switch { 0 => BookedStatus, 1 => AvailableStatus, _ => SoldStatus },
					_ => throw new NotImplementedException(nameof(seatStatus))
				};
			}

			return payment;
		}

		private static Dictionary<GetValuesSuites, IList<CartItem>> suiteCartItemsMap => new()
		{
			{ GetValuesSuites.OneValue, [new()] },
			{ GetValuesSuites.ManyValues, ListGenerator.Generate<CartItem>(GetValuesSuites.ManyValues) }
		};

		private static string BookedStatus => SeatStatus.Booked.ToString().ToLowerInvariant();
		private static string SoldStatus => SeatStatus.Sold.ToString().ToLowerInvariant();
		private static string AvailableStatus => SeatStatus.Available.ToString().ToLowerInvariant();
	}
}
