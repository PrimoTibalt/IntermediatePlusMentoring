using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PaymentApplication
{
	public static class ServiceCollectionExtension
	{
		public static void AddPaymentApplication(this IServiceCollection services)
		{
			services.TryAddTransient<IMediator, Mediator>();
			services.Register();
		}
	}
}