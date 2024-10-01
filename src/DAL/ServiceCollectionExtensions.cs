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
			services.TryAddScoped<IGenericRepository<Venue.Venue>, VenuesRepository>();
			services.TryAddScoped<IGenericRepository<Venue.Section>, SectionsRepository>();
			services.TryAddScoped<IGenericRepository<Venue.Row>, RowsRepository>();
			services.TryAddScoped<IGenericRepository<Venue.Seat>, SeatsRepository>();
		}

		public static void AddEventsRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Event>, EventRepository>();
			services.TryAddScoped<IGenericRepository<EventSeat>, EventSeatRepository>();
			services.TryAddScoped<IGenericRepository<Manifest>, ManifestRepository>();
			services.TryAddScoped<IGenericRepository<Price>, PriceRepository>();
		}

		public static void AddOrderRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<ICartRepository, CartRepository>();
			services.TryAddScoped<IGenericRepository<CartItem>, CartItemRepository>();
			services.TryAddScoped<IGenericRepository<User>, UserRepository>();
		}

		public static void AddPaymentRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Payment.Payment>, PaymentRepository>();
		}
	}
}
