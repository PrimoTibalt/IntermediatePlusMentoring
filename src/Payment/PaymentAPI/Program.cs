using System.Text.Json.Serialization;
using PaymentApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(cfg =>
	{
		cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});
builder.Services.AddPaymentApplication(builder.Configuration);

var app = builder.Build();
app.MapControllers();
app.Run();

