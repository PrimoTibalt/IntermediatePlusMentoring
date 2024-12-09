using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace DAL.Payments.Repository
{
	[DapperAot]
	public sealed class DapperPaymentRepository : IDapperPaymentRepository
	{
		private const string getByIdQuery = """
			select p."Id", p."Status", p."CartId" 
			from public."Payments" p
			where p."Id" = @id
			""";
		private readonly string _connectionString;

		public DapperPaymentRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public Task<bool> CompletePayment(long id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> FailPayment(long id)
		{
			throw new NotImplementedException();
		}

		public async Task<Payment> GetById(long id)
		{
			using var connection = new NpgsqlConnection(_connectionString);
			return await connection.QueryFirstAsync<Payment>(getByIdQuery,
				new { id }, commandType: CommandType.Text);
		}

		public Task<Payment> GetPaymentWithRelatedInfo(long id)
		{
			throw new NotImplementedException();
		}
	}
}
