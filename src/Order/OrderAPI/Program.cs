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

var app = builder.Build();
app.MapControllers();
app.Run();

public partial class Program { }
