using DAL.Orders.Repository;
using Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace DAL.Orders.Strategies
{
	internal class OptimisticConcurrencyBookingStrategy(ICartRepository cartRepository) : IExecutionStrategy<Guid>
	{
		private readonly ICartRepository _cartRepository = cartRepository;

		public async Task<bool> Execute(Guid id)
		{
			try
			{
				var seats = (await _cartRepository.GetItemsWithEventSeat(id)).Select(ci => ci.EventSeat).ToList();
				if (seats.Any(s => s.Status != (int)SeatStatus.Available))
					return false;

				foreach (var seat in seats)
				{
					seat.Status = (int)SeatStatus.Booked;
				}

				var updateCount = await _cartRepository.Save();

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
