using EventApplication;
using System.Text.Json.Serialization;
using Events = EventApplication.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(cfg =>
{
	cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEventApplication(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
