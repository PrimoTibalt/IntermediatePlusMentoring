using MediatR;
using Notifications.Infrastructure.Services;
using PaymentApplication.Commands;
using PaymentApplication.Queries;

namespace PaymentAPI.Endpoints
{
	public static class PaymentEndpoints
	{
		public static void RegisterPaymentEndpoint(this IEndpointRouteBuilder builder)
		{
			var group = builder.MapGroup("/payments/{id:long}");
			group.MapGet("", GetPaymentById);

			group.MapPost("/failed", FailPayment);

			group.MapPost("/complete", CompletePayment);
		}

		public static Task<IResult> CompletePayment(long id, IMediator mediator, INotificationService<long> notificationService) 
			=> OperateOnPayment(id, true, mediator, notificationService);

		public static Task<IResult> FailPayment(long id, IMediator mediator, INotificationService<long> notificationService)
			=> OperateOnPayment(id, false, mediator, notificationService);

		public static async Task<IResult> GetPaymentById(long id, IMediator mediator)
		{
				var result = await mediator.Send(new GetPaymentQuery { Id = id });
				if (result is null) return TypedResults.NotFound($"Payment with id '{id}' was not found.");
				return Results.Ok(result);
		}

		private static async Task<IResult> OperateOnPayment(long id, bool completePayment, IMediator mediator, INotificationService<long> notificationService)
		{
			try
			{
				var result = await mediator.Send(new ProcessPaymentCommand { Id = id, Complete = completePayment });
				if (result is null) return TypedResults.BadRequest($"Payment with id '{id}' doesn't exist or operation can't be performed on it.");
				if (result.Success)
				{
					await notificationService.SendNotification(id);
					return Results.Ok();
				}
				else
				{
					return TypedResults.BadRequest("Unable to finish operation, check status of tickets.");
				}
			}
			catch (Exception e)
			{
				return TypedResults.BadRequest(e.Message);
			}
		}
	}
}
