using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Polly;
using Testcontainers.PostgreSql;

namespace OrderTests
{
	public class EndToEndExecutioner
	{
		private readonly IList<IContainer> containers;
		private readonly ResiliencePipeline _resiliencePipeline;

		public EndToEndExecutioner()
		{
			containers = new List<IContainer>();
			containers.Add(new PostgreSqlBuilder()
				.WithImage("postgres:17.0")
				.WithPortBinding(5404, 5432)
				.WithEnvironment("POSTGRES_PASSWORD", "postgres")
				.WithResourceMapping(new FileInfo("init.sql"), "/docker-entrypoint-initdb.d/")
				.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
				.WithCleanUp(true)
				.WithAutoRemove(true)
				.Build());
			containers.Add(new ContainerBuilder()
				.WithImage("redis:7.0")
				.WithPortBinding(6380, 6379)
				.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
				.WithCleanUp(true)
				.WithAutoRemove(true)
				.Build());
			_resiliencePipeline = new ResiliencePipelineBuilder()
				.AddRetry(new() { Delay = TimeSpan.FromSeconds(10), MaxRetryAttempts = 10 })
				.AddTimeout(TimeSpan.FromSeconds(30))
				.Build();
		}

		public async Task Execute(Func<Task> action)
		{
			await _resiliencePipeline.ExecuteAsync(async (token) =>
			{
				await Parallel.ForEachAsync(containers, async (container, token) =>
				{
					await container.StartAsync(token);
				});
				await action();
			});
			await Parallel.ForEachAsync(containers, async (container, token) =>
			{
				await container.StopAsync(token);
				await container.DisposeAsync();
			});
		}
	}
}
