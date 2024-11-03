using DAL.Events;
using DAL.Orders.Repository;

namespace DAL.Orders.Strategies
{
	internal class OptimisticConcurrencyBookingStrategy : IExecutionStrategy<Guid>
	{
		private readonly ICartRepository _cartRepository;

		public OptimisticConcurrencyBookingStrategy(ICartRepository cartRepository)
		{
			_cartRepository = cartRepository;
		}

		public async Task<bool> Execute(Guid id)
		{
			try
			{
				await _cartRepository.BeginTransaction();

				var seats = (await _cartRepository.GetItemsWithEventSeat(id)).Select(ci => ci.EventSeat).ToList();
				if (seats.Any(s => !string.Equals(s.Status, SeatStatus.Available.ToString(),
					StringComparison.OrdinalIgnoreCase)))
					return false;

				foreach (var seat in seats)
				{
					seat.Status = SeatStatusStrings.Booked;
				}

				var updateCount = await _cartRepository.Save();

				await _cartRepository.CommitTransaction();
				return true;
			}
			catch (InvalidOperationException)
			{
				return false;
			}
		}
	}
}
