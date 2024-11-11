using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications.Infrastructure;
using RabbitMQ.Client;

namespace NotificationsHandler
{
	internal class Program
	{
		public static async Task Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureServices((builder, services) =>
				{
					var factory = new ConnectionFactory
					{
						Uri = new(builder.Configuration.GetConnectionString("RabbitConnection"))
					};
					services.AddNotificationConnectionProvider(factory);
					services.AddHostedService<QueueSubscriberService>();
				})
				.Build();

			await host.RunAsync();
		}
	}
}
