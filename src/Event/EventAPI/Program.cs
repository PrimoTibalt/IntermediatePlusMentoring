using EventApplication;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(cfg =>
{
	cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEventApplication(builder.Configuration);

var app = builder.Build();
app.MapControllers();
app.Run();
