using DAL.Events;
using DAL.Events.Repository;
using DAL.Orders;
using DAL.Payments;
using DAL.Venues;
using DAL.Venues.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DAL
{
	public static class ServiceCollectionExtensions
	{
		public static void AddVenuesRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IVenueRepository, VenueRepository>();
			services.TryAddScoped<ISectionRepository, SectionRepository>();
			services.TryAddScoped<IGenericRepository<Row, int>, GenericRepository<Row, int, VenueContext>>();
			services.TryAddScoped<IGenericRepository<Seat, int>, GenericRepository<Seat, int, VenueContext>>();
		}

		public static void AddEventsRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IEventRepository, EventRepository>();
			services.TryAddScoped<IEventSeatRepository, EventSeatRepository>();
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
