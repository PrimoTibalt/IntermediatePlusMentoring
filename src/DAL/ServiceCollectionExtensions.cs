using DAL.Events;
using DAL.Events.Repositories;
using DAL.Orders;
using DAL.Orders.Repositories;
using DAL.Payments;
using DAL.Payments.Repositories;
using DAL.Venues;
using DAL.Venues.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DAL
{
	public static class ServiceCollectionExtensions
	{
		public static void AddVenuesRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Venue, int>, VenuesRepository>();
			services.TryAddScoped<IGenericRepository<Section, int>, SectionsRepository>();
			services.TryAddScoped<IGenericRepository<Row, int>, RowsRepository>();
			services.TryAddScoped<IGenericRepository<Seat, int>, SeatsRepository>();
		}

		public static void AddEventsRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Event, int>, EventRepository>();
			services.TryAddScoped<IGenericRepository<EventSeat, long>, EventSeatRepository>();
			services.TryAddScoped<IGenericRepository<Price, int>, PriceRepository>();
		}

		public static void AddOrderRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Cart, Guid>, CartRepository>();
			services.TryAddScoped<IGenericRepository<CartItem, long>, CartItemRepository>();
			services.TryAddScoped<IGenericRepository<User, int>, UserRepository>();
		}

		public static void AddPaymentRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Payment, long>, PaymentRepository>();
		}
	}
}
