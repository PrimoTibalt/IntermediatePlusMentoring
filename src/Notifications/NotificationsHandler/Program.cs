using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications.Infrastructure;
using NotificationsHandler.Providers;
using NotificationsHandler.QueuesServices;
using RabbitMQ.Client;
using Resend;

namespace NotificationsHandler
{
	internal class Program
	{
		public static async Task Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureServices((builder, services) =>
				{
					services.AddNotificationContext(builder.Configuration);
					services.AddNotificationRepositories();
					var factory = new ConnectionFactory
					{
						Uri = new(builder.Configuration.GetConnectionString("RabbitConnection"))
					};
					services.AddNotificationConnectionProvider(factory);
					services.AddOptions();
					services.AddHttpClient<ResendClient>();
					services.Configure<ResendClientOptions>(o =>
					{
						o.ApiToken = builder.Configuration["ResendApiToken"];
					});
					services.AddTransient<IResend, ResendClient>();
					services.AddTransient<INotificationProvider, EmailProvider>();
					services.AddHostedService<NotificationQueueSubscriberService>();
				})
				.Build();

			await host.RunAsync();
		}
	}
}
