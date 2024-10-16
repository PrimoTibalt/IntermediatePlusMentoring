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

			CreateMap<Section, SectionDetails>()
				.ForMember(sd => sd.Rows, o => o.MapFrom(s => s.Rows.Select(r => r.Id)));
			
			CreateMap<EventSeat, SeatDetails>()
				.ForMember(sd => sd.RowId, o => o.MapFrom(es => es.Seat.RowId))
				.ForMember(sd => sd.SectionId, o => o.MapFrom(es => es.Seat.Row.SectionId))
				.ForMember(sd => sd.VenueSeatId, o => o.MapFrom(es => es.SeatId));
		}
	}
}
