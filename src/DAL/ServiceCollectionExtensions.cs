using DAL.Events;
using DAL.Events.Repository;
using DAL.Orders;
using DAL.Orders.Repository;
using DAL.Payments;
using DAL.Payments.Repository;
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
			services.TryAddScoped<Events.Repository.IEventSeatRepository, Events.Repository.EventSeatRepository>();
		}

		public static void AddOrderRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<ICartRepository, CartRepository>();
			services.TryAddScoped<ICartItemRepository, CartItemRepository>();
			services.TryAddScoped<Orders.Repository.IEventSeatRepository, Orders.Repository.EventSeatRepository>();
			services.TryAddScoped<IGenericRepository<Payment, long>, GenericRepository<Payment, long, OrderContext>>();
		}

		public static void AddPaymentRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IPaymentRepository, PaymentRepository>();
		}
	}
}
