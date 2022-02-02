using System;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application.Repository
{
	public interface IVenueRepository
	{
		Venue GetById(VenueId id);
		void Add(Venue venue);
	}
}
