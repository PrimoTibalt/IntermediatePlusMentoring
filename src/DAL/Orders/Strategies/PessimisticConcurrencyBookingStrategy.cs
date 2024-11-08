using DAL.Events;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DAL.Orders.Strategies
{
	internal class PessimisticConcurrencyBookingStrategy(OrderContext context) : IExecutionStrategy<Guid>
	{
		// Usually it seats in sproc on db side to synchronize changes
		private const string seatsLockQuery = """
			SELECT es."Id", es."Status"
			FROM public."CartItems" ci 
			JOIN public."EventSeats" es ON es."Id" = ci."EventSeatId"
			WHERE ci."CartId" = @CartId
			FOR UPDATE
			""";
		private const string bookSeatsQuery = """
			UPDATE public."EventSeats"
			SET "Status" = @BookedStatus
			WHERE "Id" = ANY(@SeatIds)
			""";

		private readonly OrderContext _context = context;

		public async Task<bool> Execute(Guid id)
		{
			var connection = _context.Database.GetDbConnection();
			var transaction = _context.Database.CurrentTransaction.GetDbTransaction();
			try
			{
				var seats = await connection.QueryAsync<(long Id, int Status)>(
					seatsLockQuery,
					new { CartId = id },
					transaction);
				if (seats.Any(s => s.Status != (int)SeatStatus.Available))
					return false;

				await connection.ExecuteAsync(
					bookSeatsQuery,
					new { SeatIds = seats.Select(s => s.Id).ToList(), BookedStatus = (int)SeatStatus.Booked },
					transaction);

				return true;
			}
			catch
			{
				await transaction.RollbackAsync();
				return false;
			}
		}
	}
}
