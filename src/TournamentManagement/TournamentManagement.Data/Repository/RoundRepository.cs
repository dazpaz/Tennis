using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.RoundAggregate;

namespace TournamentManagement.Data.Repository
{
	public class RoundRepository : IRoundRepository
	{
		private readonly TournamentManagementDbContext _context;

		public RoundRepository(TournamentManagementDbContext context)
		{
			_context = context;
		}

		public Round GetById(RoundId id)
		{
			var round = _context.Rounds.Find(id);
			return round;
		}
		
		public void Add(Round round)
		{
			_context.Rounds.Add(round);
		}
	}
}
