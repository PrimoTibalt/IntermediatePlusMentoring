using RabbitMQ.Client;

namespace Notifications.Infrastructure.Providers
{
    public interface IChannelProvider
    {
        Task<IChannel> GetChannel();
    }
}