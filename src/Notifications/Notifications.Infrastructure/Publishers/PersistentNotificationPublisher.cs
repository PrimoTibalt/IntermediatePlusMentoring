using DAL.Notifications;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Models;
using Npgsql;
using RegisterServicesSourceGenerator;
using System.Data;
using System.Text.Json;

namespace Notifications.Infrastructure.Publishers
{
	[RegisterService<IPersistentNotificationPublisher>(LifeTime.Scoped)]
	internal class PersistentNotificationPublisher(INotificationsPublisher publisher,
		IServiceScopeFactory scopeFactory,
		ILogger<PersistentNotificationPublisher> logger)
		: IPersistentNotificationPublisher
	{
		private const string insertNotificationEntityCommand = """
			INSERT INTO public."Notifications" ("Id", "Timestamp", "Status", "Data")
			VALUES (@Id, @CreatedAt, @Status, @Data);
			""";
		private readonly INotificationsPublisher _publisher = publisher;
		private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
		private readonly ILogger _logger = logger;

		[DapperAot]
		public async Task PersistentPublish(Notification notification, string queue)
		{
			_ = notification ?? throw new ArgumentNullException(nameof(notification));

			using var stream = new MemoryStream();
			JsonSerializer.Serialize(stream, notification, NotificationSerializationContext.Default.Notification);

			var parameters = new DynamicParameters();
			parameters.Add("Id", notification.Id, DbType.Guid);
			parameters.Add("CreatedAt", notification.Timestamp, DbType.DateTime);
			parameters.Add("Status", (int)NotificationStatus.InProgress, DbType.Int32);
			parameters.Add("Data", stream.ToArray(), DbType.Binary);
			var notificationEntity = new NotificationEntity
			{
				Id = notification.Id,
				Timestamp = notification.Timestamp,
				Status = (int)NotificationStatus.InProgress,
				Data = stream.ToArray()
			};
			try
			{
				using var scope = _scopeFactory.CreateScope();
				var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
				using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
				await connection.OpenAsync();
				await connection.ExecuteAsync(insertNotificationEntityCommand, parameters, commandType: CommandType.Text);
				await _publisher.SendMessage(notificationEntity.Data, queue);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to create persistent notification.");
			}
		}
	}
}
