using AutoMapper;
using DAL.Payments;
using PaymentApplication.Entities;

namespace PaymentApplication.Core
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Payment, PaymentDetails>();
		}
	}
}
