using Entities.Events;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using Entities.Payments;
using PaymentApplication.Repository;

namespace DAL.Payments.Repository
{
	[DapperAot]
	public sealed class DapperPaymentRepository : IDapperPaymentRepository
	{
		private const string getByIdQuery = """
			SELECT p."Id", p."Status", p."CartId" 
			FROM public."Payments" p
			WHERE p."Id" = @id
			""";

		private const string getAmountByPaymentIdQuery = """
			SELECT SUM(price."Price")
			FROM public."Prices" price
			JOIN public."CartItems" ci
				ON price."Id" = ci."PriceId"
			JOIN public."Payments" p
				ON ci."CartId" = p."CartId"
			WHERE p."Id" = @id
			""";

		private const string getEventsByPaymentIdQuery = """
			SELECT e."Name"
			FROM public."Events" e
			JOIN public."EventSeats" es
				ON es."EventId" = e."Id"
			JOIN public."CartItems" ci
				ON ci."EventSeatId" = es."Id"
			JOIN public."Payments" p
				ON p."CartId" = ci."CartId"
			WHERE p."Id" = @id
			""";

		private const string getPaymentSummaryQuery = """
			SELECT u."Email", p."Status"
			FROM public."Payments" as p
			JOIN public."Carts" as c
				ON c."Id" = p."CartId"
			JOIN public."Users" as u
				ON u."Id" = c."UserId"
			WHERE p."Id" = @id
			""";

		private readonly string _connectionString;

		public DapperPaymentRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task<bool> CompletePayment(long id)
		{
			var command = GetPaymentCommand(
				[(int)SeatStatus.Booked],
				(int)PaymentStatus.Completed,
				(int)SeatStatus.Sold
			);
			using var connection = new NpgsqlConnection(_connectionString);
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();
			try
			{
				await connection.ExecuteAsync(command, new { id }, transaction, commandType: CommandType.Text);
				await transaction.CommitAsync();
				return true;
			}
			catch
			{
				await transaction.RollbackAsync();
				return false;
			}
		}

		public async Task<bool> FailPayment(long id)
		{
			var command = GetPaymentCommand(
				[(int)SeatStatus.Booked, (int)SeatStatus.Available],
				(int)PaymentStatus.Failed,
				(int)SeatStatus.Available
			);
			using var connection = new NpgsqlConnection(_connectionString);
			await connection.OpenAsync();
			using var transaction = connection.BeginTransaction();
			try
			{
				await connection.ExecuteAsync(command, new { id }, transaction, commandType: CommandType.Text);
				await transaction.CommitAsync();
				return true;
			}
			catch
			{
				await transaction.RollbackAsync();
				return false;
			}
		}

		public async Task<Payment> GetById(long id)
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QueryFirstAsync<Payment>(getByIdQuery,
				new { id }, commandType: CommandType.Text);
		}

		internal readonly record struct PaymentData(int Status, string Email);
		public async Task<PaymentSummary> GetPaymentSummary(long id)
		{
			using var connection = new NpgsqlConnection(_connectionString);

			var (Status, Email) = await connection.QueryFirstAsync<PaymentData>(getPaymentSummaryQuery,
				new { id }, commandType: CommandType.Text);
			var paymentSummary = new PaymentSummary
			{
				Id = id,
				Amount = await connection.QueryFirstAsync<decimal>(getAmountByPaymentIdQuery, new { id }, commandType: CommandType.Text),
				Events = [.. await connection.QueryAsync<string>(getEventsByPaymentIdQuery, new { id }, commandType: CommandType.Text)],
				Status = Status,
				UserEmail = Email
			};

			return paymentSummary;
		}

		private string GetPaymentCommand(int[] expectedSeatsStatus, int paymentStatus, int seatsStatus)
		{
			var statusSelector = string.Join(" OR ", expectedSeatsStatus.Select(status =>
			$$"""
				es."Status" = {{status}}
			"""));
			var query = $$"""
			SELECT
				es."Id"
			FROM public."EventSeats" es
			JOIN public."CartItems" ci
				ON es."Id" = ci."EventSeatId"
			JOIN public."Payments" p
				ON ci."CartId" = p."CartId"
			WHERE p."Id" = @id AND ({{statusSelector}});

			UPDATE public."Payments"
			SET "Status" = {{paymentStatus}}
			WHERE "Id" = @id;

			UPDATE public."EventSeats"
			SET "Status" = {{seatsStatus}}
			WHERE "Id" IN 
				(SELECT ci."EventSeatId"
				FROM public."CartItems" ci
				LEFT JOIN public."Payments" p
					ON ci."CartId" = p."CartId"
				WHERE p."Id" = @id);
			""";

			return query;
		}
	}
}
