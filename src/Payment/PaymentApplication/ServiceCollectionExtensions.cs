using DAL;
using DAL.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Services;
using PaymentApplication.Commands;
using PaymentApplication.Core;
using PaymentApplication.Entities;
using PaymentApplication.Handlers;
using PaymentApplication.Notifications;
using PaymentApplication.Queries;
using RabbitMQ.Client;

namespace PaymentApplication
{
	public static class ServiceCollectionExtension
	{
		public static void AddPaymentApplication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<PaymentContext>(options =>
			{
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddPaymentRepositories(configuration);
			services.AddAutoMapper(config => config.AddProfile(new MappingProfiles()));
			var factory = new ConnectionFactory
			{
				Uri = new(configuration.GetConnectionString("RabbitConnection"))
			};
			services.AddNotificationConnectionProvider(factory);
			services.TryAddScoped<INotificationService<long>, PaymentNotificationService>();
			services.TryAddTransient<IMediator, Mediator>();
			services.TryAddTransient<IRequestHandler<GetPaymentQuery, PaymentDetails>, GetPaymentHandler>();
			services.TryAddTransient<IRequestHandler<ProcessPaymentCommand, bool>, ProcessPaymentHandler>();
		}
	}
}