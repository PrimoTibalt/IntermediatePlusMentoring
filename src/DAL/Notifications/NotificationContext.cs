using Microsoft.EntityFrameworkCore;

namespace DAL.Notifications
{
	public class NotificationContext(DbContextOptions options) : DbContext(options)
	{
		public DbSet<NotificationEntity> Notifications { get; set; }
	}
}
