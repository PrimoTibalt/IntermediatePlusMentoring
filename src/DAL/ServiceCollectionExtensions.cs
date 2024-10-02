using DAL.Events;
using DAL.Events.Repositories;
using DAL.Order;
using DAL.Order.Repositories;
using DAL.Payment.Repositories;
using DAL.Venue.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DAL
{
	public static class ServiceCollectionExtensions
	{
		public static void AddVenuesRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Venue.Venue, int>, VenuesRepository>();
			services.TryAddScoped<IGenericRepository<Venue.Section, int>, SectionsRepository>();
			services.TryAddScoped<IGenericRepository<Venue.Row, int>, RowsRepository>();
			services.TryAddScoped<IGenericRepository<Venue.Seat, int>, SeatsRepository>();
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
			services.TryAddScoped<IGenericRepository<Payment.Payment, long>, PaymentRepository>();
		}
	}
}
