using Entities.Notifications;
using Microsoft.EntityFrameworkCore;

namespace DAL.Notifications
{
	internal class NotificationContext(DbContextOptions options) : DbContext(options)
	{
		public DbSet<NotificationEntity> Notifications { get; set; }
	}
}
