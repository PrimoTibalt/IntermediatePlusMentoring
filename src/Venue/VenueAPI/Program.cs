using DAL;
using System.Text.Json.Serialization;
using VenueApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});

builder.Services.AddVenuesContext(builder.Configuration);
builder.Services.AddVenuesRepositories();
builder.Services.AddVenueApplicaiton();

var app = builder.Build();
app.MapControllers();
app.Run();
