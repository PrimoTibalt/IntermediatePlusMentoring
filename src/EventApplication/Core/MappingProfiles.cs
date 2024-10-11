using AutoMapper;
using DAL.Events;
using DAL.Venues;
using EventApplication.Entities;

namespace EventApplication.Core
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles() 
		{
			CreateMap<Event, EventDetails>()
				.ForMember(ed => ed.VenueAddress, o => o.MapFrom(e => e.Venue.Address))
				.ForMember(ed => ed.VenueName, o => o.MapFrom(e => e.Venue.Name))
				.ForMember(ed => ed.VenueDescription, o => o.MapFrom(e => e.Venue.Description))
				.ForMember(ed => ed.Sections, o => o.MapFrom(e => e.Venue.Sections.Select(s => s.Id).ToList()));

			CreateMap<Section, Section>()
				.ForMember(s => s.Venue, o => o.Ignore());
		}
	}
}
