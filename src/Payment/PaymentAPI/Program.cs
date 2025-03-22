using System.Diagnostics;
using System.Text.Json.Serialization;
using DAL;
using Dapper;
using Notifications.Infrastructure;
using Notifications.Payments;
using PaymentAPI.Endpoints;
using PaymentApplication;
using PaymentApplication.Entities;
using RabbitMQ.Client;

[module: DapperAot]

var startTime = Stopwatch.GetTimestamp();
var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(config =>
{
	config.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonOptionsContext.Default);
});
builder.Services.AddPaymentContext(builder.Configuration);
builder.Services.AddPaymentRepositories();
builder.Services.AddPaymentApplication();
var factory = new ConnectionFactory
{
	Uri = new(builder.Configuration.GetConnectionString("RabbitConnection"))
};
builder.Services.AddNotificationConnectionProvider(factory);
builder.Services.AddPaymentNotifications();
var app = builder.Build();

app.RegisterPaymentEndpoint();
app.Lifetime.ApplicationStarted.Register(() =>
{
	Console.WriteLine(Stopwatch.GetElapsedTime(startTime));
});

app.Run();

[JsonSerializable(typeof(PaymentDetails))]
[JsonSerializable(typeof(string))]
internal partial class AppJsonOptionsContext : JsonSerializerContext
{ }
