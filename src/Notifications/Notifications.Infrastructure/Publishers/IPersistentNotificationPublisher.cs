﻿using Notifications.Infrastructure.Models;

namespace Notifications.Infrastructure.Publishers
{
	public interface IPersistentNotificationPublisher
	{
		Task PersistentPublish(Notification notification, string queue);
	}
}
