using DAL.Events.Repository;
using DAL.Notifications;
using DAL.Orders;
using DAL.Orders.Repository;
using DAL.Orders.Strategies;
using DAL.Payments.Repository;
using DAL.Venues.Repository;
using Microsoft.Extensions.Configuration;
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
			services.TryAddScoped<Orders.Repository.IPaymentRepository, Orders.Repository.PaymentRepository>();
			services.TryAddScoped<OptimisticConcurrencyBookingStrategy>();
			services.TryAddScoped<PessimisticConcurrencyBookingStrategy>();
			services.TryAddScoped<IBookCartOperation, BookCartOperation>();
			services.TryAddScoped<IGenericRepository<NotificationEntity, Guid>, GenericRepository<NotificationEntity, Guid, OrderContext>>();
		}

		public static void AddPaymentRepositories(this IServiceCollection services, IConfiguration config)
		{
			services.TryAddScoped<IDapperPaymentRepository>(serviceProvider => new DapperPaymentRepository(config));
		}

		public static void AddNotificationRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<NotificationEntity, Guid>, GenericRepository<NotificationEntity, Guid, NotificationContext>>();
		}
	}
}
