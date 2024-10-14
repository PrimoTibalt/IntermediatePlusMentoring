using AutoMapper;
using DAL.Orders;
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
        }
    }
}