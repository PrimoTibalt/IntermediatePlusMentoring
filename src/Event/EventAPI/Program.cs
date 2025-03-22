using DAL;
using EventApplication;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(cfg =>
{
	cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEventsContext(builder.Configuration);
builder.Services.AddEventsRepositories();
builder.Services.AddEventApplication();
builder.Services.AddStackExchangeRedisCache(options =>
{
	options.ConfigurationOptions = new()
	{
		AbortOnConnectFail = true,
		EndPoints = { builder.Configuration.GetConnectionString("RedisConnection") }
	};
});

var app = builder.Build();
app.MapControllers();
app.Run();
