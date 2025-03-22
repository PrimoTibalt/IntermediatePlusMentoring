using AutoMapper;
using Entities.Events;
using Entities.Orders;
using OrderApplication.Entities;

namespace OrderApplication.Core
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<CartDetails, CartDetails>();
			CreateMap<CartItem, CartItem>()
				.ForMember(ci => ci.Cart, o => o.MapFrom(ci => (Cart)null));
			CreateMap<EventSeat, EventSeatDetails>();
			CreateMap<CartItem, CartItemDetails>()
				.ForMember(cid => cid.Price, s => s.MapFrom(ci => ci.Price.Sum));
		}
	}
}