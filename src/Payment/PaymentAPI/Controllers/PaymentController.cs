using DAL.Payments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payments = PaymentApplication.Payments;

namespace PaymentAPI.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(Payment), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new Payments.Details.Query { Id = id });
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost("{id:long}/failed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Fail(long id)
        {
            return await OperateOnPayment(id, false);
        }

        [HttpPost("{id:long}/complete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Complete(long id)
        {
            return await OperateOnPayment(id, true);
        }


        public async Task<IActionResult> OperateOnPayment(long id, bool complete)
        {
            bool result;
            try 
            {
                result = await _mediator.Send(new Payments.Operate.Command { Id = id, Complete = complete });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (result) return Ok();
            return BadRequest($"Payment with id '{id}' doesn't exist or operation can't be performed on it.");
        }
    }
}