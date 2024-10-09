using System.Text.Json.Serialization;
using VenueApplication;
using VenueApplication.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});

builder.Services.AddVenueApplicaiton(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(VenueDetails).Assembly));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
