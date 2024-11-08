using DAL.Events;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace DAL.Orders.Strategies
{
	internal class PessimisticConcurrencyBookingStrategy : IExecutionStrategy<Guid>
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

		private readonly OrderContext _context;

		public PessimisticConcurrencyBookingStrategy(OrderContext context)
		{
			_context = context;
		}

		public async Task<bool> Execute(Guid id)
		{
			var connection = _context.Database.GetDbConnection();
			connection.Open();
			using var transaction = await connection.BeginTransactionAsync(IsolationLevel.RepeatableRead);

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

				await transaction.CommitAsync();

				return true;
			}
			catch (PostgresException)
			{
				await transaction.RollbackAsync();
				return false;
			}
		}
	}
}
