using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Data.Repository
{
	public class VenueRepository : IVenueRepository
	{
		private readonly TournamentManagementDbContext _context;

		public VenueRepository(TournamentManagementDbContext context)
		{
			_context = context;
		}

		public Venue GetById(VenueId id)
		{
			var venue = _context.Venues.Find(id);
			if (venue == null) return null;

			_context.Entry(venue).Collection(x => x.Courts).Load();

			return venue;
		}

		public void Add(Venue venue)
		{
			_context.Venues.Add(venue);
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
