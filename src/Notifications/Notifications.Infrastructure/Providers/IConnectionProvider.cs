using RabbitMQ.Client;

namespace Notifications.Infrastructure.Provider
{
    public interface IConnectionProvider
    {
        Task<IConnection> GetConnection();
    }
}
