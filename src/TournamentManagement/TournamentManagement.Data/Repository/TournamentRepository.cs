using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Data.Repository
{
	public class TournamentRepository : Repository<Tournament, TournamentId>
	{
		private readonly TournamentManagementDbContext _context;

		public TournamentRepository(TournamentManagementDbContext context) : base(context)
		{
			_context = context;
		}

		public override Tournament GetById(TournamentId id)
		{
			var tournament = _context.Tournaments.Find(id);
			if (tournament == null) return null;

			_context.Entry(tournament).Collection(x => x.Events).Load();

			return tournament;
		}
	}
}
