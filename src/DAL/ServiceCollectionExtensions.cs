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
			services.TryAddScoped<IGenericRepository<Venue, int>>(provider => new BaseRepository<Venue, int, VenueContext>(provider.GetRequiredService<VenueContext>(), nameof(VenueContext.Venues)));
			services.TryAddScoped<IGenericRepository<Section, int>>(provider => new BaseRepository<Section, int, VenueContext>(provider.GetRequiredService<VenueContext>(), nameof(VenueContext.Sections)));
			services.TryAddScoped<IGenericRepository<Row, int>>(provider => new BaseRepository<Row, int, VenueContext>(provider.GetRequiredService<VenueContext>(), nameof(VenueContext.Rows)));
			services.TryAddScoped<IGenericRepository<Seat, int>>(provider => new BaseRepository<Seat, int, VenueContext>(provider.GetRequiredService<VenueContext>(), nameof(VenueContext.Seats)));
		}

		public static void AddEventsRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Event, int>>(provider => new BaseRepository<Event, int, EventContext>(provider.GetRequiredService<EventContext>(), nameof(EventContext.Events)));
			services.TryAddScoped<IGenericRepository<EventSeat, long>>(provider => new BaseRepository<EventSeat, long, EventContext>(provider.GetRequiredService<EventContext>(), nameof(EventContext.EventSeats)));
			services.TryAddScoped<IGenericRepository<Price, int>>(provider => new BaseRepository<Price, int, EventContext>(provider.GetRequiredService<EventContext>(), nameof(EventContext.Prices)));
		}

		public static void AddOrderRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Cart, Guid>>(provider => new BaseRepository<Cart, Guid, OrderContext>(provider.GetRequiredService<OrderContext>(), nameof(OrderContext.Carts)));
			services.TryAddScoped<IGenericRepository<CartItem, long>>(provider => new BaseRepository<CartItem, long, OrderContext>(provider.GetRequiredService<OrderContext>(), nameof(OrderContext.CartItems)));
			services.TryAddScoped<IGenericRepository<User, int>>(provider => new BaseRepository<User, int, OrderContext>(provider.GetRequiredService<OrderContext>(), nameof(OrderContext.Users)));
		}

		public static void AddPaymentRepositories(this IServiceCollection services)
		{
			services.TryAddScoped<IGenericRepository<Payment, long>>(provider => new BaseRepository<Payment, long, PaymentContext>(provider.GetRequiredService<PaymentContext>(), nameof(PaymentContext.Payments)));
		}
	}
}
