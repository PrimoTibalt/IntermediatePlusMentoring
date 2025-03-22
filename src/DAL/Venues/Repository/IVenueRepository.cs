﻿using DAL.Abstraction;
using Entities.Venues;

namespace DAL.Venues.Repository
{
	public interface IVenueRepository : IGenericRepository<Venue, int>
	{
	}
}
