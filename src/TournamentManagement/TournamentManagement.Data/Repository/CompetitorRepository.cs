using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.CompetitorAggregate;

namespace TournamentManagement.Data.Repository
{
	public class CompetitorRepository : ICompetitorRepository
	{
		private readonly TournamentManagementDbContext _context;

		public CompetitorRepository(TournamentManagementDbContext context)
		{
			_context = context;
		}

		public Competitor GetById(CompetitorId id)
		{
			var competitor = _context.Competitors.Find(id);
			return competitor;
		}

		public void Add(Competitor competitor)
		{
			_context.Add(competitor);
		}
	}
}
