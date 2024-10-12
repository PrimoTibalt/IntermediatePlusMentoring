using AutoMapper;
using DAL.Venues;
using VenueApplication.Entities;

namespace VenueApplication.Core
{
	public sealed class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Venue, VenueDetails>()
				.ForMember(vd => vd.Sections, o => o.MapFrom(v => v.Sections.Select(s => s.Id)));

			CreateMap<Section, SectionDetails>()
				.ForMember(sd => sd.Rows, o => o.MapFrom(s => s.Rows.Select(r => r.Id)));

			CreateMap<Row, RowDetails>()
				.ForMember(rd => rd.Seats, o => o.MapFrom(r => r.Seats.Select(s => s.Id)));
		}
	}
}
