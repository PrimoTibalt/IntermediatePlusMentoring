using API.Abstraction.Helpers;
using DAL.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.DTOs;
using Carts = OrderApplication.Carts;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Route("orders/carts")]
    public class CartsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly LinkGenerator _linkGenerator;

        public CartsController(IMediator mediator, LinkGenerator linkGenerator)
        {
            _mediator = mediator;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Resource<IList<CartItem>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetItems(Guid id)
        {
            var items = await _mediator.Send(new Carts.Items.Query { Id = id });
            if (items is null) return NotFound();
            var resource = new Resource<IList<CartItem>>
            {
                Value = items,
                Links = [
                    new Link 
                    {
                        Href = _linkGenerator.GetUriByAction(HttpContext, nameof(AddToCart), values: new { id }),
                        Method = "POST"
                    }
                ]
            }; 
            return Ok(resource);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddToCart(CartItemInputModel item, [FromQuery] Guid id)
        {
            var result = await _mediator.Send(new Carts.Add.Command { CartId = id, EventId = item.EventId, SeatId = item.SeatId, UserId = item.UserId });
            if (result is null) return NotFound("Seat not found");
            if (result.Error) return BadRequest(result.ErrorMessage);
            return CreatedAtAction(nameof(GetItems), new { id });
        }
    }
}