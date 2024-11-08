using DAL.Events;
using DAL.Orders.Repository;
using Microsoft.EntityFrameworkCore;

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
				if (seats.Any(s => s.Status != (int)SeatStatus.Available))
					return false;

				foreach (var seat in seats)
				{
					seat.Status = (int)SeatStatus.Booked;
				}

				var updateCount = await _cartRepository.Save();

				await _cartRepository.CommitTransaction();
				return true;
			}
			catch (InvalidOperationException ex) when (ex.InnerException is DbUpdateException)
			{
				return false;
			}
			catch (Exception ex)
			{
				// Better than nothing?
				Console.WriteLine("Unexpected exception has occured.");
				Console.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
