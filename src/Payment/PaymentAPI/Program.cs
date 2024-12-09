using System.Diagnostics;
using System.Text.Json.Serialization;
using Dapper;
using PaymentAPI.Endpoints;
using PaymentApplication;
using PaymentApplication.Entities;

[module: DapperAot]

var startTime = Stopwatch.GetTimestamp();
var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(config =>
{
	config.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonOptionsContext.Default);
});
builder.Services.AddPaymentApplication(builder.Configuration);

var app = builder.Build();

app.RegisterPaymentEndpoint();
app.Lifetime.ApplicationStarted.Register(() =>
{
	Console.WriteLine(Stopwatch.GetElapsedTime(startTime));
});

app.Run();

[JsonSerializable(typeof(PaymentDetails))]
internal partial class AppJsonOptionsContext : JsonSerializerContext
{ }
