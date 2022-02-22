using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Data.Repository
{
	public class TournamentRepository : ITournamentRepository
	{
		private readonly TournamentManagementDbContext _context;

		public TournamentRepository(TournamentManagementDbContext context)
		{
			_context = context;
		}

		public Tournament GetById(TournamentId id)
		{
			var tournament = _context.Tournaments.Find(id);
			if (tournament == null) return null;

			_context.Entry(tournament).Collection(x => x.Events).Load();

			return tournament;
		}

		public void Add(Tournament tournament)
		{
			_context.Tournaments.Add(tournament);
		}

		public IReadOnlyList<Tournament> GetList()
		{
			return _context.Tournaments.Include(t => t.Venue).ToList();
		}
	}
}
