using System.Text.Json.Serialization;
using OrderApplication;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(cfg =>
	{
		cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});
builder.Services.AddOrderApplication(builder.Configuration);
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));
builder.Services.AddScoped<IDatabase>(services => services.GetService<IConnectionMultiplexer>().GetDatabase());

var app = builder.Build();
app.MapControllers();
app.Run();

