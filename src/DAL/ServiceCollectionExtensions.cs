using DAL.Events;
using DAL.Orders;
using DAL.Payments;
using DAL.Venues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DAL
{
	public static class ServiceCollectionExtensions
	{
		public static void AddVenuesRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Venue, int>, GenericRepository<Venue, int, VenueContext>>();
			services.TryAddScoped<IGenericRepository<Section, int>, GenericRepository<Section, int, VenueContext>>();
			services.TryAddScoped<IGenericRepository<Row, int>, GenericRepository<Row, int, VenueContext>>();
			services.TryAddScoped<IGenericRepository<Seat, int>, GenericRepository<Seat, int, VenueContext>>();
		}

		public static void AddEventsRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Event, int>, GenericRepository<Event, int, EventContext>>();
			services.TryAddScoped<IGenericRepository<EventSeat, long>, GenericRepository<EventSeat, long, EventContext>>();
			services.TryAddScoped<IGenericRepository<Price, int>, GenericRepository<Price, int, EventContext>>();
		}

		public static void AddOrderRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Cart, Guid>, GenericRepository<Cart, Guid, OrderContext>>();
			services.TryAddScoped<IGenericRepository<CartItem, long>, GenericRepository<CartItem, long, OrderContext>>();
			services.TryAddScoped<IGenericRepository<User, int>, GenericRepository<User, int, OrderContext>>();
		}

		public static void AddPaymentRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Payment, long>, GenericRepository<Payment, long, PaymentContext>>();
		}
	}
}
