using DAL.Orders.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Orders.Repository
{
	internal class BookCartOperation : IBookCartOperation
	{
		private readonly IServiceProvider _serviceProvider;

		public BookCartOperation(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task<bool> TryBookCart(Guid id, bool optimistic)
		{
			IExecutionStrategy<Guid> strategy = optimistic
				? _serviceProvider.GetRequiredService<OptimisticConcurrencyBookingStrategy>()
				: _serviceProvider.GetRequiredService<PessimisticConcurrencyBookingStrategy>();
			return await strategy.Execute(id);
		}
	}
}
