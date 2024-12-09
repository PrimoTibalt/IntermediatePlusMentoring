using MediatR;
using PaymentApplication.Commands;
using PaymentApplication.Queries;

namespace PaymentAPI.Endpoints
{
	public static class PaymentEndpoints
	{
		public static void RegisterPaymentEndpoint(this IEndpointRouteBuilder builder)
		{
			var group = builder.MapGroup("/payments/{id:long}");
			group.MapGet("", static async (long id, IMediator mediator) =>
			{
				var result = await mediator.Send(new GetPaymentQuery { Id = id });
				if (result is null) return TypedResults.NotFound($"Payment with id '{id}' was not found.");
				return Results.Ok(result);
			});

			group.MapPost("/failed", static async (long id, bool complete, IMediator mediator)
				=> await OperateOnPayment(id, false, mediator));

			group.MapPost("/complete", static async (long id, IMediator mediator)
				=> await OperateOnPayment(id, true, mediator));
		}

		private static async Task<IResult> OperateOnPayment(long id, bool completePayment, IMediator mediator)
		{
			try
			{
				var result = await mediator.Send(new ProcessPaymentCommand { Id = id, Complete = completePayment });
				if (result) return Results.Ok();
				else return TypedResults.BadRequest($"Payment with id '{id}' doesn't exist or operation can't be performed on it.");
			}
			catch (Exception e)
			{
				return TypedResults.BadRequest(e.Message);
			}
		}
	}
}
