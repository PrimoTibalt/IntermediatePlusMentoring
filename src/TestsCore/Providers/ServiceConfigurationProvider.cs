using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestsCore.Providers
{
    public static class ServiceConfigurationProvider
    {
        public static IServiceProvider Get<T>(Action<IServiceCollection> registerDal) where T : DbContext
        {
            var builder = DbContextOptionsBuilderProvider<T>.Get();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped(provider => (T)Activator.CreateInstance(typeof(T), builder.Options));
            registerDal(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }
    }
}
