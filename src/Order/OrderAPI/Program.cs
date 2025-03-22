using System.Text.Json.Serialization;
using Cache.Infrastructure;
using DAL;
using Notifications.Order;
using OrderApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(cfg =>
	{
		cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});

builder.Services.AddOrderContext(builder.Configuration);
builder.Services.AddOrderRepositories();
builder.Services.AddOrderApplication();
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddBookingNotificationService(builder.Configuration);

var app = builder.Build();
app.MapControllers();
app.Run();

public partial class Program { }
